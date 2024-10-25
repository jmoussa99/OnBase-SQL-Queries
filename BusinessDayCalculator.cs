// Skeleton generated by Hyland Unity Editor on 2/12/2023 7:43:08 PM
namespace EndDateCalculation
{
    using System;
    using System.Text;
    using Hyland.Unity;
    using Hyland.Unity.CodeAnalysis;
    using Hyland.Unity.Workflow;
 using System.Collections.Generic;
    
    
    /// <summary>
    /// EndDate Calculation
    /// </summary>
    public class EndDateCalculation : Hyland.Unity.IWorkflowScript
    {
        Diagnostics.DiagnosticsLevel v = Diagnostics.DiagnosticsLevel.Verbose;  //level 0
        Diagnostics.DiagnosticsLevel i = Diagnostics.DiagnosticsLevel.Info;     //level 1
        Diagnostics.DiagnosticsLevel w = Diagnostics.DiagnosticsLevel.Warning;  //Level 2
        Diagnostics.DiagnosticsLevel e = Diagnostics.DiagnosticsLevel.Error;    //Level 3
  
  string str_ScriptName = "EndDate Calculation (v8)";
        #region IWorkflowScript
        /// <summary>
        /// Implementation of <see cref="IWorkflowScript.OnWorkflowScriptExecute" />.
        /// <seealso cref="IWorkflowScript" />
        /// </summary>
        /// <param name="app"></param>
        /// <param name="args"></param>
        public void OnWorkflowScriptExecute(Hyland.Unity.Application app, Hyland.Unity.WorkflowEventArgs args)
        {
            // Add Code Here
      app.Diagnostics.WriteIf(v, string.Format("{0} starting at: {1}", str_ScriptName, DateTime.Now));
      DateTime endDate;
      DateTime startDate;
      DayOfWeek saturday = DayOfWeek.Saturday;
            DayOfWeek sunday = DayOfWeek.Sunday;
      List<DateTime> holidays = new List<DateTime>();
   DateTime dueDate;
      args.ScriptResult = false;
   int totalBusinessDays = 0;
      int daysCount = 0;
   
      try{
       if(!args.PropertyBag.TryGetValue("startDate", out startDate)){
         app.Diagnostics.Write("Could not get startDate");
         return;
       }
       if(!args.PropertyBag.TryGetValue("daysCount", out daysCount)){
         app.Diagnostics.Write("Could not get startDate");
         return;
       }
    
       int currentYear = DateTime.Today.Year;
                int startYear = startDate.Year;

                holidays.AddRange(NewYears(startYear, currentYear + 1));
                holidays.AddRange(PresidentsDays(startYear, currentYear + 1));
                holidays.AddRange(MemorialDays(startYear, currentYear + 1));
                holidays.AddRange(IndependenceDays(startYear, currentYear + 1));
                holidays.AddRange(LaborDays(startYear, currentYear + 1));
                holidays.AddRange(ThanksgivingDays(startYear, currentYear + 1));
                holidays.AddRange(ChristmasDays(startYear, currentYear + 1));
    
    //if start date on non business day, break processing.
    if(startDate.DayOfWeek == saturday || startDate.DayOfWeek == sunday){
     app.Diagnostics.Write("Start Date on Weekend or Holiday: " + startDate.ToShortDateString());
     return;
    }
       
//    foreach(var holiday in holidays){
//     app.Diagnostics.Write("Holiday:" + holiday.ToString());
//    } 
    
       int counter = 0;
                while (counter < daysCount)
                {
                    DayOfWeek day = startDate.DayOfWeek;
                    if (day != saturday && day != sunday)
                    {
                        counter++;
      totalBusinessDays++;
                    }
     //app.Diagnostics.Write("Updating end date: " + startDate);
                    startDate = startDate.AddDays(1);
     app.Diagnostics.Write("New end date: " + startDate);

                    while (startDate.DayOfWeek == saturday || startDate.DayOfWeek == sunday)
                    {
      
      if(startDate.DayOfWeek == saturday || startDate.DayOfWeek == sunday){
       app.Diagnostics.Write("Date on Weekend");
      }
      //app.Diagnostics.Write("Updating end date: " + startDate);
                        startDate = startDate.AddDays(1);
      app.Diagnostics.Write("New end date: " + startDate);
                    }

                }
       endDate = startDate;
                args.PropertyBag.Set("endDate", endDate.ToShortDateString());
                args.ScriptResult = true;
    
    //update due date if on holiday or weekend
    if(args.PropertyBag.TryGetValue("MonthlyDueDate", out dueDate)){
     //app.Diagnostics.Write("Updating Monthly Due Date: " + dueDate.ToString());
     while (dueDate.DayOfWeek == saturday || dueDate.DayOfWeek == sunday){
      app.Diagnostics.Write("New Monthly Due Date: " + dueDate.ToString());
      dueDate = dueDate.AddDays(-1);
     }
     args.PropertyBag.Set("MonthlyDueDate", dueDate.ToShortDateString());
    }
    if(args.PropertyBag.TryGetValue("yrDate", out dueDate)){
     //app.Diagnostics.Write("Updating Yearly Due Date: " + dueDate.ToString());
     while (dueDate.DayOfWeek == saturday || dueDate.DayOfWeek == sunday){
      app.Diagnostics.Write("New Yearly Due Date: " + dueDate.ToString());
      dueDate = dueDate.AddDays(-1);
     }
     args.PropertyBag.Set("yrDate", dueDate.ToShortDateString());
    }
     args.PropertyBag.Set("totalDays", totalBusinessDays.ToString());
      }
    catch(Exception ex){
      args.ScriptResult = false;
      app.Diagnostics.Write(ex);
     }
    app.Diagnostics.WriteIf(v, string.Format("{0} ending at: {1}", str_ScriptName, DateTime.Now));
  }
   
        private List<DateTime> NewYears(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("1-1-" + startYear.ToString());
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        break;
                    case DayOfWeek.Saturday:
                        dt = dt.AddDays(-1);  //moved observded day to a weekday
                        break;
                    case DayOfWeek.Sunday:
                        dt = dt.AddDays(1);  //moved observded day to a weekday
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> PresidentsDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("2-1-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                        dt = DateTime.Parse("02-15-" + startYear);
                        break;
                    case DayOfWeek.Tuesday:
                        dt = DateTime.Parse("02-21-" + startYear);
                        break;
                    case DayOfWeek.Wednesday:
                        dt = DateTime.Parse("02-20-" + startYear);
                        break;
                    case DayOfWeek.Thursday:
                        dt = DateTime.Parse("02-19-" + startYear);
                        break;
                    case DayOfWeek.Friday:
                        dt = DateTime.Parse("02-18-" + startYear);
                        break;
                    case DayOfWeek.Saturday:
                        dt = DateTime.Parse("02-17-" + startYear);
                        break;
                    case DayOfWeek.Sunday:
                        dt = DateTime.Parse("02-16-" + startYear);
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> MemorialDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("May-01-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                        dt = DateTime.Parse("05-29-" + startYear);
                        break;
                    case DayOfWeek.Tuesday:
                        dt = DateTime.Parse("05-28-" + startYear);
                        break;
                    case DayOfWeek.Wednesday:
                        dt = DateTime.Parse("05-27-" + startYear);
                        break;
                    case DayOfWeek.Thursday:
                        dt = DateTime.Parse("05-26-" + startYear);
                        break;
                    case DayOfWeek.Friday:
                        dt = DateTime.Parse("05-25-" + startYear);
                        break;
                    case DayOfWeek.Saturday:
                        dt = DateTime.Parse("05-31-" + startYear);
                        break;
                    case DayOfWeek.Sunday:
                        dt = DateTime.Parse("05-30-" + startYear);
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> IndependenceDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("7-4-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        break;
                    case DayOfWeek.Saturday:
                        dt = dt.AddDays(-1);  //moved observded day to a weekday
                        break;
                    case DayOfWeek.Sunday:
                        dt = dt.AddDays(1);  //moved observded day to a weekday
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> UtahPioneerDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("7-24-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        break;
                    case DayOfWeek.Saturday:
                        dt = dt.AddDays(-1);  //moved observded day to a weekday
                        break;
                    case DayOfWeek.Sunday:
                        dt = dt.AddDays(1);  //moved observded day to a weekday
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> LaborDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("9-1-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                        dt = DateTime.Parse("9-7-" + startYear);
                        break;
                    case DayOfWeek.Wednesday:
                        dt = DateTime.Parse("9-6-" + startYear);
                        break;
                    case DayOfWeek.Thursday:
                        dt = DateTime.Parse("9-5-" + startYear);
                        break;
                    case DayOfWeek.Friday:
                        dt = DateTime.Parse("9-4-" + startYear);
                        break;
                    case DayOfWeek.Saturday:
                        dt = DateTime.Parse("9-3-" + startYear);
                        break;
                    case DayOfWeek.Sunday:
                        dt = DateTime.Parse("9-2-" + startYear);
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> ThanksgivingDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("11-1-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                        dt = DateTime.Parse("11-25-" + startYear);
                        break;
                    case DayOfWeek.Tuesday:
                        dt = DateTime.Parse("11-24-" + startYear);
                        break;
                    case DayOfWeek.Wednesday:
                        dt = DateTime.Parse("11-23-" + startYear);
                        break;
                    case DayOfWeek.Thursday:
                        dt = DateTime.Parse("11-22-" + startYear);
                        break;
                    case DayOfWeek.Friday:
                        dt = DateTime.Parse("11-28-" + startYear);
                        break;
                    case DayOfWeek.Saturday:
                        dt = DateTime.Parse("11-27-" + startYear);
                        break;
                    case DayOfWeek.Sunday:
                        dt = DateTime.Parse("11-26-" + startYear);
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                DateTime blackFriday = dt.AddDays(1);
                //comment out if the day after thenksgiving is not a holiday. 
                if (!oReturn.Contains(blackFriday))
                    oReturn.Add(blackFriday);

                startYear++;
            }

            return oReturn;
        }
        private List<DateTime> ChristmasDays(int startYear, int endYear)
        {
            List<DateTime> oReturn = new List<DateTime>();
            while (startYear <= endYear)
            {
                DateTime dt = DateTime.Parse("12-25-" + startYear);
                DayOfWeek day = dt.DayOfWeek;
                switch (day)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        break;
                    case DayOfWeek.Saturday:
                        dt = dt.AddDays(-1); //moved observded day to a weekday
                        break;
                    case DayOfWeek.Sunday:
                        dt = dt.AddDays(1);  //moved observded day to a weekday
                        break;
                }
                if (!oReturn.Contains(dt))
                    oReturn.Add(dt);
                startYear++;
            }

            return oReturn;
        }
        #endregion
    }
}