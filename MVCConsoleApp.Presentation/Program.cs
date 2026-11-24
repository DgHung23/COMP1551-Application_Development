// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// Console application entry point (Presentation layer).
//
// Architecture (3 tiers + MVC inside the console application):
//   Presentation  : Views (console I/O only) + Controllers (menu actions)
//   Business      : Services + validation / business rules
//   Data Access   : Repositories (all SQL) + Entities (the Model)
//
// This file is the "composition root": it creates every object once and
// connects them (dependency injection by constructor), then starts the menu.
// =====================================================================

using System.Text;
using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Presentation.Controllers;
using MVCConsoleApp.Presentation.Views;

// UTF-8 so names with accents (e.g. Vietnamese) and the £ sign display correctly.
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
Console.Title = "Music Festival Operations System";

// ---------- Business layer services (they create their own repositories) ----------
IPersonService personService = new PersonService();
ILookupService lookupService = new LookupService();
IReportService reportService = new ReportService();

// ---------- Views ----------
var menuView = new MenuView();
var personView = new PersonView();
var reportView = new ReportView();

// ---------- Controllers ----------
var personController = new PersonController(personService, lookupService, personView);
var reportController = new ReportController(reportService, personService, reportView, menuView);
var mainController = new MainController(menuView, personController, reportController);

// ---------- Start ----------
mainController.Run();
