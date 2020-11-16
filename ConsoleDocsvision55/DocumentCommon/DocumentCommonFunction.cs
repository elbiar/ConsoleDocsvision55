using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.Platform.ObjectManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConsoleDocsvision55.DocumentCommon
{
    internal class DocumentCommonFunction
    {
        public DocumentCommonFunction()
        { }

        public void GetDataApproval( Guid cardId)
        {
            CardData document = DvConnection.UserSession.CardManager.GetCardData(cardId);
            var name = document.Sections[CardDocument.MainInfo.ID].FirstRow.GetString("Name");
            //секция ЛС
            var rows=document.Sections[new Guid("{9D216EB2-D63B-4436-8620-C4A1ED66D738}")].Rows;
            var data=rows.GroupBy(s => s.GetString("ФИОЛС"))
                .Select(s => s.OrderByDescending(d => d.GetDateTime("ДатаЛС2"))
                .First())
                .ToList()
                .OrderBy( t => t.GetDateTime("ДатаЛС2") ) ;

            foreach( var item in data)
            {
                var employee=item.GetString("ФИОЛС");
                var post = item.GetString("ДолжностьЛС");
                var date = item.GetDateTime("ДатаЛС2");
                var res= item.GetString("РезультатЛС");
                var comment = item.GetString("КомментарийЛС");
                Console.WriteLine($"{employee} {post} {date} {res} {comment}");
             }
            Console.ReadKey();
        }
    }
}
