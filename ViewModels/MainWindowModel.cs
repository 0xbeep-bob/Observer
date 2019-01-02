using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Observer.EFModels;
using Observer.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using Observer.Views;   // этого пространства имен не должно быть тут в соответствии с MVVM
using xNet;
using System.Net;

namespace Observer.ViewModels
{
    class MainWindowModel : PropertyChangedBase, IDataErrorInfo
    {
        #region Object

        // контексты
        MyBaseContext mycontext = new MyBaseContext();
        CCPBaseContext ccpcontext = new CCPBaseContext();

        // подключение
        Sourse sourse = new Sourse();

        // инициализация списка бонусов структур
        List<EngineeringComplexes> engin = new List<EngineeringComplexes>()
        {
            new EngineeringComplexes { Name = "none", MEBonus = 0, TEBonus = 0, TaxBonus = 0},
            new EngineeringComplexes { Name = "Raitaru", MEBonus = 1, TEBonus = 15, TaxBonus = 3},
            new EngineeringComplexes { Name = "Azbel", MEBonus = 1, TEBonus = 20, TaxBonus = 4},
            new EngineeringComplexes { Name = "Sotiyo", MEBonus = 1, TEBonus = 30, TaxBonus = 5}
        };

        // инициализация списка уровней ресерча
        List<ResearchLevelModifier> researchlevel = new List<ResearchLevelModifier>()
        {
            new ResearchLevelModifier { ME = 0, TE = 0, LevelModifier = 0},
            new ResearchLevelModifier { ME = 1, TE = 2, LevelModifier = 105},
            new ResearchLevelModifier { ME = 2, TE = 4, LevelModifier = 250},
            new ResearchLevelModifier { ME = 3, TE = 6, LevelModifier = 595},
            new ResearchLevelModifier { ME = 4, TE = 8, LevelModifier = 1414},
            new ResearchLevelModifier { ME = 5, TE = 10, LevelModifier = 3360},
            new ResearchLevelModifier { ME = 6, TE = 12, LevelModifier = 8000},
            new ResearchLevelModifier { ME = 7, TE = 14, LevelModifier = 19000},
            new ResearchLevelModifier { ME = 8, TE = 16, LevelModifier = 45255},
            new ResearchLevelModifier { ME = 9, TE = 18, LevelModifier = 107700},
            new ResearchLevelModifier { ME = 10, TE = 20, LevelModifier = 256000}
        };

        #endregion

        #region Field      

        int bpID;   // id чертежа
        int sysID;  // id industry системы 

        decimal rigMultiply;                  // множитель эффективности рига от CC системы
        decimal levelModifierMEResearch;      // дельта уровней: левел прокачки - текущий
        decimal levelModifierTEResearch;      // дельта уровней: левел прокачки - текущий
        int indicatorbp;                      // поле для проверки изменения ID чертежа в запросе
        int indicatorsys;                     // поле для проверки изменения ID системы в запросе
        string indicatorcitadel;              // поле для проверки изменения цитадели в запросе
        int secManufact;                      // время производства
        int secCopying;                       // время копирования

        decimal baseprice;  // базовая цена, импорт с xml

        // коллекция для хранения индексов
        Dictionary<string, decimal> indicesManufact;

        #endregion

        #region Property

        // к имени публичного свойства биндятся свойства элементов xaml
        // свойства для конструктора
        public List<string> Items { get; set; }
        public List<string> Systems { get; set; }

        public string pIDBlueprint { get; set; }
        public List<Datagrid> DataGrigList { get; set; }
        public List<Datagrid> DataGrigRawList { get; set; }
        public List<Datagrid> TempList { get; set; }
        public string pIndustrySystemName { get; set; } = "Jita";

        // свойства GroupBox "Product info"
        public string ProductName { get; set; }
        public int ItemPerRun { get; set; }
        public int LimitRunPerCopy { get; set; }
        public decimal SellPrice { get; set; } = 0m;
        public decimal BuyPrice { get; set; } = 0m;

        // бонусы структуры
        public string StructName { get; set; } = "none";
        public string StructME { get; set; }
        public string StructTE { get; set; }

        // левел ресерча чертежа
        public decimal ExpectedMEResearch { get; set; } = 10m;
        public decimal ExpectedTEResearch { get; set; } = 20m;

        // для расчета времени
        public int DaysManufact { get; set; } = 0;
        public int HoursManufact { get; set; } = 0;
        public int MinutesManufact { get; set; } = 0;
        public int SecondsManufact { get; set; } = 0;

        public int DaysManufactRuns { get; set; } = 0;
        public int HoursManufactRuns { get; set; } = 0;
        public int MinutesManufactRuns { get; set; } = 0;
        public int SecondsManufactRuns { get; set; } = 0;

        public int DaysMEResearch { get; set; } = 0;
        public int HoursMEResearch { get; set; } = 0;
        public int MinutesMEResearch { get; set; } = 0;
        public int SecondsMEResearch { get; set; } = 0;

        public int DaysTEResearch { get; set; } = 0;
        public int HoursTEResearch { get; set; } = 0;
        public int MinutesTEResearch { get; set; } = 0;
        public int SecondsTEResearch { get; set; } = 0;

        public int DaysResearch { get; set; } = 0;
        public int HoursResearch { get; set; } = 0;
        public int MinutesResearch { get; set; } = 0;
        public int SecondsResearch { get; set; } = 0;

        public int DaysCopying { get; set; } = 0;
        public int HoursCopying { get; set; } = 0;
        public int MinutesCopying { get; set; } = 0;
        public int SecondsCopying { get; set; } = 0;

        public int DaysCopyingRuns { get; set; } = 0;
        public int HoursCopyingRuns { get; set; } = 0;
        public int MinutesCopyingRuns { get; set; } = 0;
        public int SecondsCopyingRuns { get; set; } = 0;

        // свойства GroupBox "Bonuses"
        public decimal OrigMEManufact { get; set; } = 0m;
        public decimal OrigTEManufact { get; set; } = 0m;
        public decimal RigMEManufact { get; set; } = 0m;
        public decimal RigTEManufact { get; set; } = 0m;
        public decimal ImplantManufactTE { get; set; } = 0m;
        public decimal IndustrySkill { get; set; } = 0m;

        public decimal OrigMEResearch { get; set; } = 0m;
        public decimal OrigTEResearch { get; set; } = 0m;
        public decimal RigMEResearch { get; set; } = 0m;
        public decimal RigTEResearch { get; set; } = 0m;
        public decimal ImplantResearchME { get; set; } = 0m;
        public decimal ImplantResearchTE { get; set; } = 0m;
        public decimal MetallurgySkill { get; set; } = 0m;
        public decimal ResearchSkill { get; set; } = 0m;

        public decimal RigCopying { get; set; } = 0m;
        public decimal ImplantCopying { get; set; } = 0m;
        public decimal ScienceSkill { get; set; } = 0m;

        public decimal AdvancedIndustrySkill { get; set; } = 0m;

        public decimal OrigMEManufactComponent { get; set; } = 0m;
        public decimal RigMEManufactComponent { get; set; } = 0m;

        // свойства GroupBox "Industry Tax"
        public decimal FacilityTax { get; set; } = 0m;
        public string StructTAX { get; set; }

        public decimal IndexManufact { get; set; } = 0m;
        public decimal RigTaxManufact { get; set; } = 0m;
        public decimal GrossJobCostManufact { get; set; } = 0m;
        public decimal TotalJobCostManufact { get; set; } = 0m;

        public decimal IndexMEResearch { get; set; } = 0m;
        public decimal RigTaxMEResearch { get; set; } = 0m;
        public decimal GrossJobCostMEResearch { get; set; } = 0m;
        public decimal TotalJobCostMEResearch { get; set; } = 0m;

        public decimal IndexTEResearch { get; set; } = 0m;
        public decimal RigTaxTEResearch { get; set; } = 0m;
        public decimal GrossJobCostTEResearch { get; set; } = 0m;
        public decimal TotalJobCostTEResearch { get; set; } = 0m;

        public decimal IndexCopying { get; set; } = 0m;
        public decimal RigTaxCopying { get; set; } = 0m;
        public decimal GrossJobCostCopying { get; set; } = 0m;
        public decimal TotalJobCostCopying { get; set; } = 0m;

        // свойства GroupBox Result Manufactiring
        public decimal ResourceSellPriceTotal { get; set; } = 0m;
        public decimal ResourceBuyPriceTotal { get; set; } = 0m;
        public decimal SellProfit { get; set; } = 0m;
        public decimal BuyProfit { get; set; } = 0m;
        public decimal RawResourceSellPriceTotal { get; set; } = 0m;
        public decimal RawResourceBuyPriceTotal { get; set; } = 0m;
        public decimal RawSellProfit { get; set; } = 0m;
        public decimal RawBuyProfit { get; set; } = 0m;

        // свойства GroupBox Trader Tax
        public decimal BrokersFeeSell { get; set; } = 3.00m;
        public decimal BrokersFeeBuy { get; set; } = 3.00m;
        public decimal SalesTax { get; set; } = 2.00m;

        // свойства GroupBox Runs
        public int SliderValueManufact { get; set; } = 1;
        public int MaxSliderManufact { get; set; } = 1;
        public int SliderValueCopying { get; set; } = 1;
        public int MaxSliderCopying { get; set; } = 1;
        public int SliderValueRunsPerCopy { get; set; } = 1;

        // валидация свойств
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "SalesTax":
                        if ((SalesTax < 0.00m) || (SalesTax > 2.00m))
                        {
                            error = "SalesTax должен быть в диапазоне 0% - 2%";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Constructor

        public MainWindowModel()
        {
            // инициализация списка чертежей
            Items = mycontext.IDBlueprints.Select(c => c.ItemName).ToList();
            // инициализация списка систем
            Systems = ccpcontext.mapSolarSystems.Select(c => c.solarSystemName).ToList();
            // инициализация дефолтных бонусов цитадели чтоб в форме были нули
            StructME = "0";
            StructTE = "0";
            StructTAX = "0";
        }

        #endregion

        #region Command

        /// <summary>
        /// команда обновления базы
        /// </summary>
        public ICommand GoRefresh = new DelegateCommand(async (obj) => await Refresh.Refr());

        // вызов окна обновления базы, этот метод нарушает паттерн MVVM
        void Refr1()
        {
            var rr = new RefreshWindow();
            rr.ShowDialog();
        }

        /// <summary>
        /// команда вызова цен
        /// </summary>
        public ICommand GoStart
        {
            get { return new DelegateCommand((obj) => Start()); }
        }

        #endregion

        #region Method

        /// <summary>
        /// обновление лимита слайдера
        /// </summary>
        void GetsMaxRuns()
        {
            MaxSliderManufact = 2592000 / secManufact + 1;  // +1 указан т.к. если n-1 ран меньше 30 дней то n допускается выход за лимит 30 дней
            try
            {
                MaxSliderCopying = 2592000 / secCopying + 1;
            }
            catch
            {
                MaxSliderCopying = 0;
            }
        }

        /// <summary>
        /// запрос id чертежа по введеному имени
        /// </summary>
        int GetsMainRequest()
        {
            var blueprintRequest = from i in mycontext.IDBlueprints
                                   where i.ItemName == pIDBlueprint
                                   select i.ItemID;

            return blueprintRequest.Single();
        }

        /// <summary>
        /// запрос id системы по введеному имени (система производства)
        /// </summary>
        int GetsSystemRequest()
        {
            var systemRequest = from it in ccpcontext.mapSolarSystems
                                where it.solarSystemName == pIndustrySystemName
                                select it.solarSystemID;

            return systemRequest.Single();
        }

        /// <summary>
        /// информация о продукте
        /// </summary>
        void GetsProductInfo()
        {
            ProductName = ccpcontext.invTypes
                          .Where(i => i.typeID == ccpcontext.industryActivityProducts.Where(j => j.typeID == bpID && j.activityID == 1).Select(j => j.productTypeID).FirstOrDefault())
                          .Select(i => i.typeName)
                          .Single()
                          .ToString();

            ItemPerRun = ccpcontext.industryActivityProducts.Where(i => i.typeID == bpID && i.activityID == 1).Select(i => i.quantity).Single();

            LimitRunPerCopy = ccpcontext.industryBlueprints
                              .Where(i => i.typeID == bpID)
                              .Select(i => i.maxProductionLimit)
                              .Single();

            var productID = from i in ccpcontext.industryActivityProducts
                            where i.typeID == bpID && i.activityID == 1     // 1 - мануфакторинг
                            select i.productTypeID;

            string json = sourse.GetData("https://esi.tech.ccp.is/latest/markets/10000002/orders/?type_id=" + productID.Single().ToString());

            //десериализация в класс, в строке json массив объектов, поэтому десериализовывать нужно в список или массив
            List<PriceCCP> pullprices = JsonConvert.DeserializeObject<List<PriceCCP>>(json);

            if (pullprices != null) // будет null при отсутствии коннекта
            {
                var sellprice = from q in pullprices
                                where q.is_buy_order == false && q.system_id == 30000142     // в запросе id jita
                                select q;
                var buyprice = from q in pullprices
                               where q.is_buy_order == true && q.system_id == 30000142
                               select q;
                try
                {
                    SellPrice = sellprice.Count() != 0 ? sellprice.Min(a => a.price) : 0;
                    BuyPrice = buyprice.Count() != 0 ? buyprice.Max(a => a.price) : 0;
                }
                catch (HttpException) // доп проверка, навсякий
                {
                    SellPrice = 0;
                    BuyPrice = 0;
                    MessageBox.Show("Ошибка коннекта к серверу ссп, данные селл и бай ордера установлены в 0, повторите запрос или проверьте сеть");
                }
            }
            else // выполняется при отсутствии коннекта
            {
                SellPrice = 0;
                BuyPrice = 0;
                MessageBox.Show("Ошибка коннекта к серверу ссп, данные селл и бай ордера установлены в 0, повторите запрос или проверьте сеть");
            }
        }

        /// <summary>
        /// перевод времени в формат DD / hh:mm:ss    
        /// </summary>
        void GetsTime(int sec, out int Days, out int Hours, out int Minutes, out int Seconds)
        {
            Days = sec / 86400;

            int secOst = sec % 86400;
            Hours = secOst / 3600;
            Minutes = (secOst - Hours * 3600) / 60;
            Seconds = secOst - Hours * 3600 - Minutes * 60;
        }

        /// <summary>
        /// запрос индустриальных индексов системы и запись их в словарь   
        /// </summary>
        void GetsIndustryIndex()
        {
            string json = sourse.GetData("https://esi.tech.ccp.is/latest/industry/systems");

            List<IndustryIndeсesCCP> pullprices = JsonConvert.DeserializeObject<List<IndustryIndeсesCCP>>(json);

            if (pullprices != null)
            {
                foreach (IndustryIndeсesCCP i in pullprices)
                {
                    if (i.solar_system_id == sysID)
                    {
                        foreach (Index I in i.cost_indices)
                        {
                            indicesManufact.Add(I.activity, I.cost_index * 100);
                        }
                    }
                }
            }
            else
            {
                indicesManufact = new Dictionary<string, decimal>{
                    { "manufacturing", 0 },
                    { "researching_time_efficiency", 0 },
                    { "researching_material_efficiency", 0 },
                    { "copying", 0 },
                    { "reaction", 0 }
                };
            }
        }

        /// <summary>
        /// Запрос xml и baseprise
        /// </summary>
        void GestBasePriceReguest()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("https://api.eve-industry.org/job-base-cost.xml?names=" + pIDBlueprint);
                XmlElement xRoot = doc.DocumentElement;
                XmlNode childnodes = xRoot.SelectSingleNode("//job-base-cost");
                decimal.TryParse(childnodes.InnerText, NumberStyles.Number, CultureInfo.InvariantCulture, out baseprice);
            }
            catch (WebException) // выполняется при отсутствии коннекта
            {
                baseprice = 0;
                MessageBox.Show("Ошибка коннекта к серверу ссп, данные индексов установлены в 0, повторите запрос или проверьте сеть");
            }
        }

        /// <summary>
        /// расчет индустриального налога
        /// </summary>
        void GetsManufactoringTax()
        {
            IndexManufact = indicesManufact["manufacturing"];
            decimal SecurityTaxManufactoring = RigTaxManufact * rigMultiply;

            decimal afterindex = baseprice * (IndexManufact / 100);
            decimal bonusstr = afterindex * engin.Where(i => i.Name == StructName).Select(i => i.TaxBonus).Single() / 100;
            decimal bonusrig = (afterindex - bonusstr) * SecurityTaxManufactoring / 100;

            GrossJobCostManufact = afterindex - bonusstr - bonusrig;
            TotalJobCostManufact = GrossJobCostManufact * (100 + FacilityTax) / 100;
        }

        /// <summary>
        /// расчет МЕ ресерч налога
        /// </summary>
        void GetsMEResearchTax()
        {
            IndexMEResearch = indicesManufact["researching_material_efficiency"];
            decimal SecurityTaxMEResearch = RigTaxMEResearch * rigMultiply;

            decimal afterindex = baseprice * 0.02m * (IndexMEResearch / 100) * levelModifierMEResearch / 105;
            decimal bonusstr = afterindex * engin.Where(i => i.Name == StructName).Select(i => i.TaxBonus).Single() / 100;
            decimal bonusrig = (afterindex - bonusstr) * SecurityTaxMEResearch / 100;

            GrossJobCostMEResearch = afterindex - bonusstr - bonusrig;
            TotalJobCostMEResearch = GrossJobCostMEResearch * (100 + FacilityTax) / 100;
        }

        /// <summary>
        /// расчет ТЕ ресерч налога
        /// </summary>
        void GetsTEResearchTax()
        {
            IndexTEResearch = indicesManufact["researching_time_efficiency"];
            decimal SecurityTaxTEResearch = RigTaxTEResearch * rigMultiply;

            decimal afterindex = baseprice * 0.02m * (IndexTEResearch / 100) * levelModifierTEResearch / 105;
            decimal bonusstr = afterindex * engin.Where(i => i.Name == StructName).Select(i => i.TaxBonus).Single() / 100;
            decimal bonusrig = (afterindex - bonusstr) * SecurityTaxTEResearch / 100;

            GrossJobCostTEResearch = afterindex - bonusstr - bonusrig;
            TotalJobCostTEResearch = GrossJobCostTEResearch * (100 + FacilityTax) / 100;
        }

        /// <summary>
        /// расчет налога на копирование
        /// </summary>
        void GetsCopyingTax()
        {
            IndexCopying = indicesManufact["copying"];
            decimal SecurityTaxCopying = RigTaxCopying * rigMultiply;

            decimal afterindex = baseprice * 0.02m * (IndexCopying / 100) * SliderValueCopying * SliderValueRunsPerCopy;
            decimal bonusstr = afterindex * engin.Where(i => i.Name == StructName).Select(i => i.TaxBonus).Single() / 100;
            decimal bonusrig = (afterindex - bonusstr) * SecurityTaxCopying / 100;

            GrossJobCostCopying = afterindex - bonusstr - bonusrig;
            TotalJobCostCopying = GrossJobCostCopying * (100 + FacilityTax) / 100;
        }

        /// <summary>
        /// расчет итога производства
        /// </summary>
        void GetsProfitManufact()
        {
            Parallel.Invoke(
                () => SellProfit = ((SellPrice * (100 - (BrokersFeeSell + SalesTax)) / 100) * ItemPerRun - ResourceSellPriceTotal * (100 + (BrokersFeeSell + SalesTax)) / 100 - TotalJobCostManufact) / ItemPerRun,
                () => BuyProfit = ((SellPrice * (100 - (BrokersFeeSell + SalesTax)) / 100) * ItemPerRun - ResourceBuyPriceTotal * (100 + (BrokersFeeBuy + SalesTax)) / 100 - TotalJobCostManufact) / ItemPerRun,
                () =>
                {
                    switch (RawResourceSellPriceTotal)
                    {
                        case 0:
                            RawSellProfit = 0;
                            break;
                        default:
                            RawSellProfit = ((SellPrice * (100 - (BrokersFeeSell + SalesTax)) / 100) * ItemPerRun - RawResourceSellPriceTotal * (100 + (BrokersFeeSell + SalesTax)) / 100 - TotalJobCostManufact) / ItemPerRun;
                            break;
                    }
                },
                () =>
                {
                    switch (RawResourceBuyPriceTotal)
                    {
                        case 0:
                            RawBuyProfit = 0;
                            break;
                        default:
                            RawBuyProfit = ((SellPrice * (100 - (BrokersFeeSell + SalesTax)) / 100) * ItemPerRun - RawResourceBuyPriceTotal * (100 + (BrokersFeeBuy + SalesTax)) / 100 - TotalJobCostManufact) / ItemPerRun;
                            break;
                    }
                });
        }

        /// <summary>
        /// доп логика для запроса датагрида
        /// </summary>
        List<Datagrid> Method(List<Datagrid> list)
        {
            MyBaseContext mycontext = new MyBaseContext();

            foreach (Datagrid i in list)
            {
                i.Sell = Convert.ToDecimal(i.Quantity) * mycontext.Prices
                                                             .Where(o => o.ItemID == i.MaterialItemID)
                                                             .Select(o => o.Sell)
                                                             .SingleOrDefault();
                i.Buy = Convert.ToDecimal(i.Quantity) * mycontext.Prices
                                                             .Where(o => o.ItemID == i.MaterialItemID)
                                                             .Select(o => o.Buy)
                                                             .SingleOrDefault();
            }

            return list;
        }

        /// <summary>
        /// главный метод, обновление UI в соответствии с фильтрами
        /// </summary>
        void Start()
        {
            //Stopwatch sw = Stopwatch.StartNew(); // таймер для диагностики
            try
            {
                // запросы на выборку из бд в соотв со значениями комбобоксов
                bpID = GetsMainRequest();
                sysID = GetsSystemRequest();

                // проверка свойств для корректной работы
                ResourceSellPriceTotal = 0m;
                ResourceBuyPriceTotal = 0m;
                RawResourceSellPriceTotal = 0m;
                RawResourceBuyPriceTotal = 0m;
                DataGrigRawList = new List<Datagrid>();

                if (SliderValueManufact > MaxSliderManufact)
                    SliderValueManufact = MaxSliderManufact;
                if (SliderValueCopying > MaxSliderCopying)
                    SliderValueCopying = MaxSliderCopying;
                if (SliderValueRunsPerCopy > LimitRunPerCopy)
                    SliderValueRunsPerCopy = LimitRunPerCopy;

                // повторный расчет только при вводе новой системы производства
                if (sysID != indicatorsys)
                {
                    indicesManufact = new Dictionary<string, decimal>();
                    GetsIndustryIndex();
                }

                // повторный расчет только при вводе нового чертежа
                if (bpID != indicatorbp)
                {
                    SliderValueManufact = 1;
                    SliderValueCopying = 1;
                    SliderValueRunsPerCopy = 1;
                    GestBasePriceReguest();
                    GetsProductInfo();
                }

                // запрос для датагрида, выборка определенных параметров в список типа List<Datagrid>
                DataGrigList = (from i in ccpcontext.industryActivityMaterials
                                where i.typeID == bpID && i.activityID == 1        // 1 - мануфакторинг
                                select i)
                                    .Select(c => new                    // выборка через анонимные методы
                                    {
                                        itemid = c.typeID,
                                        matid = c.materialTypeID,
                                        quantity = c.quantity
                                    })
                                    .AsEnumerable()                     // преобразование коллекции IQueryable к IEnumerable
                                    .Select(cn => new Datagrid          // инициализация определенного типа
                                    {
                                        ItemID = cn.itemid,
                                        MaterialItemID = cn.matid,
                                        MaterialItemName = ccpcontext.invTypes.Where(t => t.typeID == cn.matid).Select(t => t.typeName).FirstOrDefault(),
                                        Quantity = cn.quantity
                                    }).OrderBy(i => i.MaterialItemID)
                                      .ToList();

                // расчет бонуса риг от CC системы
                var SecurityRequest = from i in ccpcontext.mapSolarSystems
                                      where i.solarSystemName == pIndustrySystemName
                                      select i.security;

                if (SecurityRequest.Single() > 0.450m)
                    rigMultiply = 1m;
                else if (SecurityRequest.Single() < 0m)
                    rigMultiply = 2.1m;
                else rigMultiply = 1.9m;

                decimal SecurityMEManufact = RigMEManufact * rigMultiply;
                decimal SecurityTEManufact = RigTEManufact * rigMultiply;
                decimal SecurityMEResearch = RigMEResearch * rigMultiply;
                decimal SecurityTEResearch = RigTEResearch * rigMultiply;
                decimal SecurityScience = RigCopying * rigMultiply;
                decimal SecurityMEManufactComponent = RigMEManufactComponent * rigMultiply;

                // расчет ME производства
                foreach (Datagrid i in DataGrigList)
                {
                    i.Quantity = Math.Ceiling(i.Quantity * (100 - OrigMEManufact) / 100 * (100 - engin.Where(j => j.Name == StructName).Select(j => j.MEBonus).Single()) / 100 * (100 - SecurityMEManufact) / 100);
                }

                // расчет датагрида для простых ресурсов
                for (int i = 0; i < DataGrigList.Count; i++)
                {
                    int resourceid = DataGrigList[i].MaterialItemID;    // нельзя указывать DataGrigList[i].MaterialItemID непосредственно в запросе
                    decimal quantitymultiply = DataGrigList[i].Quantity;

                    DataGrigRawList.AddRange((from j in ccpcontext.industryActivityMaterials
                                              where j.typeID == ccpcontext.industryActivityProducts.Where(m => m.productTypeID == resourceid).Select(m => m.typeID).FirstOrDefault() && j.activityID == 1   // 21037 // 11568 avatar bp
                                              select j)
                               .Select(c => new                    // выборка через анонимные методы
                               {
                                   itemid = c.typeID,
                                   matid = c.materialTypeID,
                                   quantity = c.quantity
                               })
                                    .AsEnumerable()                     // преобразование коллекции IQueryable к IEnumerable
                                    .Select(cn => new Datagrid          // их инициализация
                                    {
                                        ItemID = cn.itemid,
                                        MaterialItemID = cn.matid,
                                        MaterialItemName = ccpcontext.invTypes.Where(t => t.typeID == cn.matid).Select(t => t.typeName).FirstOrDefault(),
                                        Quantity = cn.quantity * quantitymultiply
                                    }).ToList());
                }

                DataGrigRawList = DataGrigRawList.GroupBy(i => i.MaterialItemID)
                                    .Select(group => new Datagrid
                                    {
                                        MaterialItemID = group.Key,
                                        MaterialItemName = group.Select(i => i.MaterialItemName).FirstOrDefault(),
                                        Quantity = group.Sum(i => i.Quantity)
                                    })
                                    .OrderBy(i => i.MaterialItemID)
                                    .ToList();

                foreach (Datagrid i in DataGrigRawList)
                {
                    i.Quantity = Math.Ceiling(i.Quantity * (100 - OrigMEManufactComponent) / 100 * (100 - engin.Where(j => j.Name == StructName).Select(j => j.MEBonus).Single()) / 100 * (100 - SecurityMEManufactComponent) / 100);
                }

                // расчет TE производства
                var timeRequestManufact = from i in ccpcontext.industryActivities
                                          where i.typeID == bpID && i.activityID == 1   // 1 - Manufactoring
                                          select i.time;

                secManufact = Convert.ToInt32(timeRequestManufact.SingleOrDefault() * (100 - OrigTEManufact) / 100 * (100 - engin.Where(i => i.Name == StructName).Select(i => i.TEBonus).Single()) / 100 * (100 - SecurityTEManufact) / 100 * (100 - ImplantManufactTE) / 100 * (100 - IndustrySkill) / 100 * (100 - AdvancedIndustrySkill) / 100);

                // расчет МЕ ресерча
                var timeRequestResesrch = from i in ccpcontext.industryActivities
                                          where i.typeID == bpID && i.activityID == 4   // 3 - TEResearch, 4 - MEResearch, время ресерча МЕ = времени ТЕ, разделение сделано в угоду фильтрам
                                          select i.time;

                levelModifierMEResearch = researchlevel.Where(i => i.ME == ExpectedMEResearch).Select(i => i.LevelModifier).Single() - researchlevel.Where(i => i.ME == OrigMEResearch).Select(i => i.LevelModifier).Single();

                int secMEResearch = Convert.ToInt32(timeRequestResesrch.SingleOrDefault() * (100 - OrigMEResearch) / 100 * (100 - engin.Where(i => i.Name == StructName).Select(i => i.TEBonus).Single()) / 100 * (100 - SecurityMEResearch) / 100 * (100 - ImplantResearchME) / 100 * (100 - MetallurgySkill) / 100 * (100 - AdvancedIndustrySkill) / 100 * levelModifierMEResearch / 105);

                // расчет TЕ ресерча   
                levelModifierTEResearch = researchlevel.Where(i => i.TE == ExpectedTEResearch).Select(i => i.LevelModifier).Single() - researchlevel.Where(i => i.TE == OrigTEResearch).Select(i => i.LevelModifier).Single();

                int secTEResearch = Convert.ToInt32(timeRequestResesrch.SingleOrDefault() * (100 - OrigTEResearch) / 100 * (100 - engin.Where(i => i.Name == StructName).Select(i => i.TEBonus).Single()) / 100 * (100 - SecurityTEResearch) / 100 * (100 - ImplantResearchTE) / 100 * (100 - ResearchSkill) / 100 * (100 - AdvancedIndustrySkill) / 100 * levelModifierTEResearch / 105);

                // расчет времени копинга
                var timeRequestCopying = from i in ccpcontext.industryActivities
                                         where i.typeID == bpID && i.activityID == 5   // 5 - Copying
                                         select i.time;

                secCopying = Convert.ToInt32(timeRequestCopying.SingleOrDefault() * (100 - engin.Where(i => i.Name == StructName).Select(i => i.TEBonus).Single()) / 100 * (100 - SecurityScience) / 100 * (100 - ImplantCopying) / 100 * (100 - ScienceSkill) / 100 * (100 - AdvancedIndustrySkill) / 100);

                // -------------------------------------- Незаконченный кусок кода о проверке категории вводимого чертежа ------------------------------------------------------

                //GetsProductInfo();

                //if (ccpcontext.invMetaTypes.Where(x => x.typeID == ccpcontext.invTypes.Where(c => c.typeName == ProductName).Select(c => c.typeID).FirstOrDefault()).Select(x => x.metaGroupID).Single() == 1)
                //{
                //    var timeRequest3 = from i in ccpcontext.industryActivities
                //                       where i.typeID == bpID && i.activityID == 5 //&& ccpcontext.invMetaTypes.Where(x => x.typeID == ccpcontext.invTypes.Where(c => c.typeName == ProductName).Select(c => c.typeID).FirstOrDefault()).Select(x => x.metaGroupID == 1).FirstOrDefault()  // 5 - Copying
                //                       select i.time;

                //    secCopying = Convert.ToInt32(timeRequest3.SingleOrDefault() * (100 - engin.Where(i => i.Name == StructName).Select(i => i.TEBonus).Single()) / 100 * (100 - SecurityScience) / 100 * (100 - ImplantCopying) / 100 * (100 - ScienceSkill) / 100 * (100 - AdvancedIndustrySkill) / 100);
                //}
                //else secCopying = 0;

                // -------------------------------------------------------------------------------------------------------------------------------------------------------------           

                // вызов вторичных методов 
                Parallel.Invoke(
                    () =>
                    {
                        DataGrigList = Method(DataGrigList);
                        foreach (Datagrid i in DataGrigList)
                        {
                            ResourceSellPriceTotal += i.Sell;
                            ResourceBuyPriceTotal += i.Buy;
                        }

                        DataGrigRawList = Method(DataGrigRawList);
                        foreach (Datagrid i in DataGrigRawList)
                        {
                            RawResourceSellPriceTotal += i.Sell;
                            RawResourceBuyPriceTotal += i.Buy;
                        }
                    },
                    () =>
                    {
                        GetsTime(secManufact, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysManufact = days;
                        HoursManufact = hours;
                        MinutesManufact = minutes;
                        SecondsManufact = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secManufact * SliderValueManufact, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysManufactRuns = days;
                        HoursManufactRuns = hours;
                        MinutesManufactRuns = minutes;
                        SecondsManufactRuns = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secMEResearch, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysMEResearch = days;
                        HoursMEResearch = hours;
                        MinutesMEResearch = minutes;
                        SecondsMEResearch = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secTEResearch, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysTEResearch = days;
                        HoursTEResearch = hours;
                        MinutesTEResearch = minutes;
                        SecondsTEResearch = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secMEResearch + secTEResearch, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysResearch = days;
                        HoursResearch = hours;
                        MinutesResearch = minutes;
                        SecondsResearch = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secCopying, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysCopying = days;
                        HoursCopying = hours;
                        MinutesCopying = minutes;
                        SecondsCopying = secondsmanufact;
                    },
                    () =>
                    {
                        GetsTime(secCopying * SliderValueCopying, out int days, out int hours, out int minutes, out int secondsmanufact);
                        DaysCopyingRuns = days;
                        HoursCopyingRuns = hours;
                        MinutesCopyingRuns = minutes;
                        SecondsCopyingRuns = secondsmanufact;
                    },
                    () =>
                    {
                        GetsManufactoringTax();
                        GetsMEResearchTax();
                        GetsTEResearchTax();
                        GetsCopyingTax();
                    },
                    () =>
                    {
                        // повторный расчет только при вводе новой цитадели постройки
                        if (StructName != indicatorcitadel)
                        {
                            StructME = engin.Where(j => j.Name == StructName).Select(j => j.MEBonus).Single().ToString();
                            StructTE = engin.Where(j => j.Name == StructName).Select(j => j.TEBonus).Single().ToString();
                            StructTAX = engin.Where(j => j.Name == StructName).Select(j => j.TaxBonus).Single().ToString();
                        }
                    }
                    );

                Task.WaitAll();

                GetsMaxRuns();
                GetsProfitManufact();
            }
            catch
            {
                MessageBox.Show("Упс, что-то пошло не так, попробуйте еще раз");
            }

            // инициализация индикаторов
            indicatorbp = bpID;
            indicatorsys = sysID;
            indicatorcitadel = StructName;

            //MessageBox.Show(sw.Elapsed.ToString());
        }

        #endregion
    }
}