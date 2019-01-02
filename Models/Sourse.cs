using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xNet;

namespace Observer.Models
{
    /// <summary>
    /// Запрос к серверу, источнику json
    /// </summary>
    class Sourse
    {
        HttpRequest request = new HttpRequest();

        public string GetData(string regueststring)
        {
            //string regueststring = "https://esi.tech.ccp.is/latest/markets/10000002/orders/?type_id=34133"; - пример запроса

            string data = ""; // не null чтобы не вызывать ArgumentNullException далее

            try
            {
                data = request.Get(regueststring).ToString();
            }
            catch
            {
                // пустой блок, обработка исключения передана дальше
            }

            return data;
        }
    }
}