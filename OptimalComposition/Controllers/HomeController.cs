using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OptimalComposition.Models;
using Microsoft.SolverFoundation.Services;


namespace OptimalComposition.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ////////////////// Поиск решений
            SolverContext problem = SolverContext.GetContext();
            Model model = problem.CreateModel();
            Decision[] component = new Decision[3];

            //// Модель
            component[0] = new Decision(Domain.RealRange(0, 9), $"Num1");
            component[1] = new Decision(Domain.RealRange(0, 9), $"Num2");
            component[2] = new Decision(Domain.RealRange(0, 9), $"Num3");
            // связываем модель
            model.AddDecisions(component);
            // Ограничения
            model.AddConstraint($"Usl1", component[0] == 3);
            model.AddConstraint($"Usl2", component[1] == 0.5);
            model.AddConstraint($"Summ5", (component[0] + component[1] * component[2]) == 5);
            // Расчет
            Solution solution = problem.Solve();
            //////////////////// Конец поиска решений


            // Сделать, что бы, если колличество требуемых стлбоцов 7, то автоматически заполняем предсгенерироваными данными
            // Начало теста
            InputValues inputValues = new InputValues();
            inputValues.ComponentInputValues = new List<ComponentInputValue>();
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Чугун литейный", PercentSi = 1.26, PercentMn = 0.68, Cost = 75.5, MinPercent = 24, MaxPercent = 50 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Чугун передельный", PercentSi = 1.08, PercentMn = 0.26, Cost = 60, MinPercent = 24, MaxPercent = 50 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Чугун зеркальный", PercentSi = 1.64, PercentMn = 1.57, Cost = 97, MinPercent = 0, MaxPercent = 100 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Лом чугунный", PercentSi = 1.5, PercentMn = 0.7, Cost = 36.2, MinPercent = 0, MaxPercent = 100 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Лом стальной", PercentSi = 0.5, PercentMn = 0.5, Cost = 34.3, MinPercent = 8, MaxPercent = 12 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Возврат", PercentSi = 0.4, PercentMn = 0.65, Cost = 36.2, MinPercent = 20, MaxPercent = 40 });
            inputValues.ComponentInputValues.Add(new ComponentInputValue() { Name = "Ферросилиций 45%", PercentSi = 52.02, PercentMn = 0.44, Cost = 120, MinPercent = 0, MaxPercent = 100 });
            //Конец теста

            return View(inputValues);
        }

        /// <summary>
        /// Контроллер вывода результатов расчетов
        /// </summary>
        /// <returns>Результаты расчета</returns>
        public IActionResult Results(InputValues data)
        {
            double FullMassComponents = data.FullMassComponents;
            data.FullMassComponents = 100;
            double Cof = FullMassComponents / 100; // Коофицент для интерполяции массы от 100
            ////////////////// Поиск решений
            SolverContext problem = SolverContext.GetContext();
            Model model = problem.CreateModel();
            Decision[] component = new Decision[8];

            for (int i = 0; i < 7; i++)
            {
                // Задаем данные модели
                component[i] = new Decision(Domain.Real, $"Num{i}");
            }

            // Общая стоимость
            component[7] = new Decision(Domain.Real, $"Cost");

            // Связываем модель и данные
            model.AddDecisions(component);

            // Ограничения
            for (int i = 0; i < 7; i++)
            {
                // Ограничение диапазон значений компонента
                model.AddConstraint($"DiaOgrEl1{i}", component[i]/(data.FullMassComponents/100) > data.ComponentInputValues[i].MinPercent & component[i] / (data.FullMassComponents / 100) < data.ComponentInputValues[i].MaxPercent);
            }

            // Чему равна общая стоимость всех компонентов
            model.AddConstraint($"DiaOgrCost", component[7] == 
                (
                component[0] * data.ComponentInputValues[0].Cost +
                component[1] * data.ComponentInputValues[1].Cost +
                component[2] * data.ComponentInputValues[2].Cost +
                component[3] * data.ComponentInputValues[3].Cost +
                component[4] * data.ComponentInputValues[4].Cost +
                component[5] * data.ComponentInputValues[5].Cost +
                component[6] * data.ComponentInputValues[6].Cost
                ));
            // Ограничение суммы компонентов
            model.AddConstraint($"DiaOgrSumm", (component[0] + component[1] + component[2] + component[3] + component[4] + component[5] + component[6]) == data.FullMassComponents);

            // Общие Содержание Si
            model.AddConstraint($"DiaOgrSi",
                (
                  (component[0] * data.ComponentInputValues[0].PercentSi / 100)
                + (component[1] * data.ComponentInputValues[1].PercentSi / 100)
                + (component[2] * data.ComponentInputValues[2].PercentSi / 100)
                + (component[3] * data.ComponentInputValues[3].PercentSi / 100)
                + (component[4] * data.ComponentInputValues[4].PercentSi / 100)
                + (component[5] * data.ComponentInputValues[5].PercentSi / 100)
                + (component[6] * data.ComponentInputValues[6].PercentSi / 100)
                ) / (data.FullMassComponents / 100)
                > data.MinPercentSi &
                (
                  (component[0] * data.ComponentInputValues[0].PercentSi / 100)
                + (component[1] * data.ComponentInputValues[1].PercentSi / 100)
                + (component[2] * data.ComponentInputValues[2].PercentSi / 100)
                + (component[3] * data.ComponentInputValues[3].PercentSi / 100)
                + (component[4] * data.ComponentInputValues[4].PercentSi / 100)
                + (component[5] * data.ComponentInputValues[5].PercentSi / 100)
                + (component[6] * data.ComponentInputValues[6].PercentSi / 100)
                )
                < data.MaxPercentSi);

            // Общие Содержание Mn
            model.AddConstraint($"DiaOgrMn",
                (
                  (component[0] * data.ComponentInputValues[0].PercentMn / 100)
                + (component[1] * data.ComponentInputValues[1].PercentMn / 100)
                + (component[2] * data.ComponentInputValues[2].PercentMn / 100)
                + (component[3] * data.ComponentInputValues[3].PercentMn / 100)
                + (component[4] * data.ComponentInputValues[4].PercentMn / 100)
                + (component[5] * data.ComponentInputValues[5].PercentMn / 100)
                + (component[6] * data.ComponentInputValues[6].PercentMn / 100)
                ) / (data.FullMassComponents / 100)
                > data.MinPercentMn &
                (
                  (component[0] * data.ComponentInputValues[0].PercentMn / 100)
                + (component[1] * data.ComponentInputValues[1].PercentMn / 100)
                + (component[2] * data.ComponentInputValues[2].PercentMn / 100)
                + (component[3] * data.ComponentInputValues[3].PercentMn / 100)
                + (component[4] * data.ComponentInputValues[4].PercentMn / 100)
                + (component[5] * data.ComponentInputValues[5].PercentMn / 100)
                + (component[6] * data.ComponentInputValues[6].PercentMn / 100)
                )
                < data.MaxPercentMn);

            // Найти минимальную цену
            model.AddGoal("goal", GoalKind.Minimize, component[7]);

            // Расчет
            Solution solution = problem.Solve();
            //////////////////// Конец поиска решений

            // Начало теста
            Results results = new Results();
            for (int i = 0; i < 7; i++)
            {
                results.ComponentResults.Add(new ComponentResult() { Name = data.ComponentInputValues[i].Name, Cost = component[i].ToDouble() * Cof * data.ComponentInputValues[i].Cost, PercentMass = component[i].ToDouble() * Cof / (data.FullMassComponents/100), FullMass = component[i].ToDouble() * Cof, MnMass = (component[i].ToDouble() * Cof * data.ComponentInputValues[i].PercentMn / 100) / (data.FullMassComponents / 100), SiMass = (component[i].ToDouble() * Cof * data.ComponentInputValues[i].PercentSi / 100) / (data.FullMassComponents / 100) });
            }
            results.AllComponentResult = results.AllResult();
            // Конец теста

            return View(results);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
