using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Observer.EFModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using xNet;

namespace Observer.Models
{
    /// <summary>
    /// Класс реализующий обновление бд
    /// </summary>
    static class Refresh
    {
        static MyBaseContext context = new MyBaseContext();
        static Stopwatch sw = Stopwatch.StartNew();

        // поля для записи количества ошибок
        public static int SellFailCount { get; private set; } = 0;
        public static int BuyFailCount { get; private set; } = 0;
        public static int ConnectionFailCount { get; private set; } = 0;

        // поле для записи результата обновления
        public static string Message { get; private set; }                

        public static async Task Refr()
        {
            // использование pLinq для обновления бд
            context.Prices.AsEnumerable().AsParallel().ForAll(n => Method(n));        // замены ItemID не происходит на порядковые
            context.SaveChanges();

            // логика обновления бд
            void Method(Price item)
            {               
                try
                {
                    Sourse sourse = new Sourse();

                    string data = sourse.GetData("https://esi.tech.ccp.is/latest/markets/10000002/orders/?type_id=" + item.ItemID.ToString());

                    List<PriceCCP> prices = JsonConvert.DeserializeObject<List<PriceCCP>>(data);

                    try
                    {
                        var sellprice = from q in prices
                                        where q.is_buy_order == false && q.system_id == 30000142    // jita                             
                                        select q;
                        item.Sell = sellprice.Min(i => i.price);
                    }
                    catch
                    {
                        SellFailCount++;
                    }

                    try
                    {
                        var buyprice = from q in prices
                                       where q.is_buy_order == true && q.system_id == 30000142    // jita                            
                                       select q;
                        item.Buy = buyprice.Max(i => i.price);
                    }
                    catch
                    {
                        BuyFailCount++;
                    }
                }

                catch (HttpException)
                {
                    ConnectionFailCount++;
                }
            }

            Message = $"База обновлена\n\nЗатраченное время : {sw.Elapsed}\n\nНеобновлено селл ордеров: {SellFailCount}\nНеобновлено бай ордеров: {BuyFailCount}\nОшибок коннекта: {ConnectionFailCount}";
        }
    }
}
