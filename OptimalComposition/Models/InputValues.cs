using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptimalComposition.Models
{
    /// <summary>
    /// Введеные даные пользователем
    /// </summary>
    public class InputValues
    {
        /// <summary>
        /// Все данные о добавленных компонентах
        /// </summary>
        public List<ComponentInputValue> ComponentInputValues { get; set; }

        /// <summary>
        /// Минимальное колличество Si в %
        /// </summary>
        public double MinPercentSi { get; set; } = 2.47;
        /// <summary>
        /// Максимальное колличество Si в %
        /// </summary>
        public double MaxPercentSi { get; set; } = 2.54;

        /// <summary>
        /// Минимальное колличество Mn в %
        /// </summary>
        public double MinPercentMn { get; set; } = 0.37;
        /// <summary>
        /// Максимальное колличество Mn в %
        /// </summary>
        public double MaxPercentMn { get; set; } = 1;

        /// <summary>
        /// Общая масса всех требуемых компонентов
        /// </summary>
        public double FullMassComponents { get; set; } = 100;
    }
}
