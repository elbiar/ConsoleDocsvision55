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

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.ObjectModel;

namespace ProcessTesting
{
    public class TestScriptKz
    {
		public void Execute(ProcessInfo process, PassState passInfo, DocsVision.Platform.ObjectModel.ObjectContext objectContext)
		{
			try
			{
                #region Переменный БП
                ProcessVariable doc = process.GetVariableByName("Документ");
				DVCard _doc = (DVCard)doc.Value;
				Guid idCard = new Guid(_doc.ID);

				ProcessVariable listEmploeesApproval = process.GetVariableByName("Согласующие лица");
				var c=listEmploeesApproval.Values.Values;
				#endregion
				Document document = objectContext.GetObject<Document>(idCard);
				var cardKind = document.SystemInfo.CardKind;
				var result = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0])["ВидДокумента"]);
				var subKindDocument = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0]).GetGuid("ВидДокумента"));
				BaseUniversalItem itemCard = objectContext.GetObject<BaseUniversalItem>(subKindDocument);

			}
			catch (Exception ex)
			{
				process.LogMessage("Ошибка выполнения скрипта:" + ex.Message);
			}
			return;
		}
	}
}
