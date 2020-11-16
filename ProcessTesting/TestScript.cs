// подключение системных библиотек
using System;
using System.Xml;

// подключение библиотек СУБП
using DocsVision.Workflow.Objects;
using DocsVision.Workflow.Runtime;
using DocsVision.Workflow.Gates;
//using DocsVision.Platform.HelperAPI;
using System.Collections.Generic;
using System.Linq;
using DocsVision.Platform.ObjectManager;

namespace ProcessTesting
{
    public class TestScript
    {
		public void Execute(ProcessInfo process, PassState passInfo)
		{
			Dictionary<int, int> pages = new Dictionary<int, int>();
            pages.Add(1, 0);        // pages =0   hour =0
            pages.Add(6, 8);        // pages <5   hour =8
            pages.Add(21, 16);          // pages <=20 hour =16
            pages.Add(200, 24);     // pages >20  hour =24	
            try
			{
				ProcessVariable doc = process.GetVariableByName("Документ");
				ProcessVariable count = process.GetVariableByName("Общее кол-во листов");
				ProcessVariable hours = process.GetVariableByName("Срок исполнения задания");
				DVCard _doc = (DVCard)doc.Value;
				CardData cardData = process.Session.CardManager.GetCardData(new Guid(_doc.ID));
				RowData row = cardData.Sections[cardData.Type.Sections["СвойстваСтруктура"].Id].FirstRow;
				int countPage = row.GetInt32("КоличествоЛистов").HasValue ? row.GetInt32("КоличествоЛистов").Value : 0;
				int countApp = row.GetInt32("ВПриложении").HasValue ? row.GetInt32("ВПриложении").Value : 0;
				count.Value = countPage + countApp;
				hours.Value =  pages.Where(s => s.Key > (int)count.Value).Select(s => s.Value).Min();
				//var message=string.Format("Общее кол {0}, срок согласования {1} ч.", count.Value, hours.Value);

				//ListHours
				ProcessVariable listHours = process.GetVariableByName("ListHours");
				process.LogMessage("Кол" + listHours.Values.Count.ToString());
				process.LogMessage(listHours.GetType().Name);

				process.LogMessage("Кол listHours.VarEnumValues.Count" + listHours.VarEnumValues.Count.ToString());
				process.LogMessage("listHours.Values");
				for (int i = 0; i < listHours.Values.Count; i++)
				{
					var value = listHours.Values[i];
					process.LogMessage(value.GetType().Name);
					process.LogMessage(value.Key.ToString());
					process.LogMessage(value.Value.ToString());
					//pages.Add((int)value.Key,(int)value.Value);

				}
				process.LogMessage("listHours.VarEnumValues");
				process.LogMessage(listHours.VarEnumValues.Keys.Count.ToString());
				//for (int i = 0; i < listHours.VarEnumValues.Count; i++)
				//{
				//	process.LogMessage(listHours.VarEnumValues[i].GetType().Name );
				//	process.LogMessage(listHours.VarEnumValues[i].NumValue.ToString());
				//	process.LogMessage(listHours.VarEnumValues[i].Value.ToString());
				//}

			}
			catch (Exception ex)
			{
				process.LogMessage("Ошибка выполнения скрипта:" + ex.Message);
			}
			return;
		}
	}
}
