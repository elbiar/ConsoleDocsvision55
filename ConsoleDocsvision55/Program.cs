using ConsoleDocsvision55.TestProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDocsvision55
{
    class Program
    {
        static void Main(string[] args)
        {

            var sessionCurrent = new SessionCurrent();
            var objectContext = sessionCurrent.Get();
            using (var userSession = sessionCurrent.GetSession())
            {
                DvConnection.ObjectContext = objectContext;
                DvConnection.UserSession = userSession;

                //DocumentCommonFunction documentService = new DocumentCommonFunction();
                //documentService.GetDataApproval(new Guid("306BBFC6-A2BD-E411-B28F-E8393517F580"));

                Guid idCard = new Guid("F65D1BC2-C8FE-4F47-AFB9-002BD89DA46C"); //Заявкка на доступ Коммиссаров

                Bpocess process = new Bpocess();
                //process.NeedApproval(idCard);
                //process.Test();
                process.testFindGroup("Договоры", "Начальник ОК(КЗ)");
                #region
                //    Dictionary<int, int> Test1 = new Dictionary<int, int>();
                //    Test1.Add(1, 0);
                //    Test1.Add(6,8);
                //    Test1.Add(21,16);  
                //    Test1.Add(300, 24);



                //    int v = 1;

                //    var result = Test1.Where(s => s.Key > v).Select(s => s.Value).ToList().Min();
                //    var resul2 = 0;

                //    for (int i = 0; i < 35; i++)
                //    {
                //        result = Test1.Where(s => s.Key > i).Select(s => s.Value).ToList().Min();
                //        resul2 = Test1.Where(s => s.Key > i).Select(s => s.Value).Min();
                //        Console.WriteLine($"В документе обзее кол {i} часы {result}  {resul2}");
                //    }
                //Console.ReadKey();
                #endregion

            }
        }
    }
}
