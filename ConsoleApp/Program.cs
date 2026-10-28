using ConsoleApp;
using System.Runtime.CompilerServices;

//InternalsVisibleTo - udostępniamy internale dla projektu testowego
[assembly: InternalsVisibleTo("ConsoleApp.Tests.xUnit")]


Garden garden = new Garden(3);
garden.GetItems();