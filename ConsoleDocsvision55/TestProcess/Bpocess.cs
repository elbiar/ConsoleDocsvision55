using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ConsoleDocsvision55.TestProcess
{

    internal class Bpocess
    {
        //Заявка КЗ внутренний, информа справочный
        Dictionary<Guid, string> dictionaryRequest = new Dictionary<Guid, string>();
        public Bpocess()
        {
            dictionaryRequest.Add(new Guid("757A0C9F-5752-4EBD-98A0-A93E11E149F2"), "Заявка на доступ к АСУП БОСС-Кадровик");
            dictionaryRequest.Add(new Guid("A61EFE96-4386-4379-BC77-40AEDB003339"), "Заявка на доступ к информационному ресурсу DocPortal");
            dictionaryRequest.Add(new Guid("45224098-79A1-4934-8362-967445D96FD6"), "Заявка на доступ к информационным ресурсам");
            dictionaryRequest.Add(new Guid("5AC8DF93-5B6A-49DE-B3B0-47C51C8F5267"), "Заявка на изменение общего списка ролей доступа  АСУП БОСС-Кадровик");
        }

        public void Get(Guid cardId)
        {
            DvConnection.ObjectContext.GetObject<Document>(cardId);
            Document document = DvConnection.ObjectContext.GetObject<Document>(cardId);
            var result = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0])["ВидДокумента"]);
            var subKindDocument = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0]).GetGuid("ВидДокумента"));
            BaseUniversalItem item = DvConnection.ObjectContext.GetObject<BaseUniversalItem>(subKindDocument);

        }

        //БП необходимость согласования
        public void NeedApproval(Guid cardId)
        {
            //DvConnection.ObjectContext.GetObject<Document>(cardId);
            Document document = DvConnection.ObjectContext.GetObject<Document>(cardId);
            var cardKind = document.SystemInfo.CardKind;
            var result = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0])["ВидДокумента"]);
            var subKindDocument = (((BaseCardSectionRow)document.GetSection(new Guid("{B13955FD-3202-44D1-92BD-6B0F6878385F}"))[0]).GetGuid("ВидДокумента"));
            BaseUniversalItem itemCard = DvConnection.ObjectContext.GetObject<BaseUniversalItem>(subKindDocument);
            //сотрудники
            IEnumerable<StaffEmployee> adminSave = GetEmployeesFromGroup("КЗ", "Администратор безопасности");
            //Директор по правовым вопросам
            IEnumerable<StaffEmployee> directorLegal = GetEmployeesFromGroup("КЗ", "Директор по правовым вопросам");
            //Начальник Отдела мотив. и орган. труда(КЗ)
            IEnumerable<StaffEmployee> headDepartMotivation = GetEmployeesFromGroup("КЗ", "Начальник Отдела мотив. и орган. труда(КЗ)");
            //Директор Корпоративного университета
            IEnumerable<StaffEmployee> directorUniver = GetEmployeesFromGroup("КЗ", "Директор Корпоративного университета");
            //Начальник управления по внутреннему аудиту
            IEnumerable<StaffEmployee> headDepartAudit = GetEmployeesFromGroup("КЗ", "Начальник управления по внутреннему аудиту");

            //
            //Начальник БК
            IEnumerable<StaffEmployee> headDepartBk = GetEmployeesFromGroup("КЗ", "Начальник БК");
            //Начальник ОК(КЗ)
            IEnumerable<StaffEmployee> headDepartOk = FindGroup("Договоры", "Начальник ОК(КЗ)");
            //Начальник расчет. отдела(КЗ)   Начальник расчет. отдела(КЗ)
            IEnumerable<StaffEmployee> headDepartCalculate = GetEmployeesFromGroup("КЗ", "Начальник расчет. отдела(КЗ)");

            //StaffEmployee adminSaveFirst = GetEmployeesFromGroup("КЗ", "Начальник расчет. отдела(КЗ)").FirstOrDefault();


            //if == Внутренние Кз and Информа справоч and ВидДокумента='%Заявка%'
            if (dictionaryRequest.ContainsKey(itemCard.GetObjectId()))
            {

            }
        }


        //Получить сотрудников из группы
        private IEnumerable<StaffEmployee> GetEmployeesFromGroup(String parentGroupName, String groupName)
        {
            IStaffService staffService = DvConnection.ObjectContext.GetService<IStaffService>();
            var group = staffService.FindGroupByName(null, parentGroupName);
            if (group == null)
                return null;
            var groupCurrent = staffService.FindGroupByName(group, groupName);
            if (groupCurrent == null)
                return null;

            IEnumerable<StaffEmployee> employees = staffService.GetGroupEmployees(groupCurrent, true, true, true);
            return employees;
        }

        private IEnumerable<StaffEmployee> FindGroup(String parentGroupName, String groupName)
        {
            IStaffService staffService = DvConnection.ObjectContext.GetService<IStaffService>();
            var group = staffService.FindGroupByName(null, "КЗ");
            if (group == null)
                return null;
            var groupLevel = group.Groups.Where(s => s.Name == parentGroupName).FirstOrDefault();
            if (groupLevel == null)
                return null;
            var groupCurrent = staffService.FindGroupByName(groupLevel, groupName);
            IEnumerable<StaffEmployee> employees = staffService.GetGroupEmployees(groupCurrent, true, true, true);
            return employees;
        }

        public void Test()
        {

            IStaffService staffService = DvConnection.ObjectContext.GetService<IStaffService>();
            List<StaffEmployee> list = new List<StaffEmployee>
            {
                staffService.Get(new Guid("15fd1862-a815-42c1-9ed9-b5f3590ef0a9")),
                staffService.Get(new Guid("4eec9347-4048-4475-9263-1aa8a298f391")),
                staffService.Get(new Guid("e1509ec6-67b3-46c3-9699-c57c14ee7a70"))
            };

            var adm = staffService.Get(new Guid("4eec9347-4048-4475-9263-1aa8a298f391"));
            var index = list.IndexOf(adm);
            if (list.Count == 0 || index == list.Count - 1)
                return;

            if (index != list.Count - 1)
            {
                list.Remove(adm);
            }
            list.Insert(list.Count, adm);


        }


        public void TestFindGroup(String parentGroupName, String groupName)
        {
            IStaffService staffService = DvConnection.ObjectContext.GetService<IStaffService>();
            var group = staffService.FindGroupByName(null, "parentGroupName");
            if (group == null)
                return;
            var groupLevel = group.Groups.Where(s => s.Name == parentGroupName).FirstOrDefault();
            if (groupLevel == null)
                return;
            var groupCurrent = staffService.FindGroupByName(groupLevel, groupName);
            IEnumerable<StaffEmployee> employees = staffService.GetGroupEmployees(groupCurrent, true, true, true);

        }



    }
}
