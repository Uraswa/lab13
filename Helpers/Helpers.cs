using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    [ExcludeFromCodeCoverage]
    public static class Helpers
    {
        private static Random rnd;
        /**
        * <summary>Фунция обработчик ввода числа типа uint.</summary>
        * <param name="keyNum">Название вводимого пользователем числа.</param>
        * <param name="minValue">Минимально допустимое значение вкл</param>
        * <param name="maxValue">Максимально допустимое значение вкл</param>
        * <returns>Гарантированно корректное число типа int, введенное пользователем.</returns>
        */
        public static uint EnterUInt(string keyNum, uint minValue = uint.MinValue, uint maxValue = uint.MaxValue)
        {
            Console.Write("Введите " + keyNum + ": ");
            while (true)
            {
                // данные пользователя.
                string readString = Console.ReadLine();
                // проверка: является ли ввод числом и не выходит ли он за пределы int32
                if (uint.TryParse(readString, out uint result))
                {
                    if (result < minValue || result > maxValue)
                    {
                        Console.Write(
                            $"Ошибка. Число должно находится в пределах [{minValue};{maxValue}], введите {keyNum} заново:");
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    Console.Write(
                        $"Ошибка. Введеные некорректные данные. Пожалуйста, введите целочисленное число в пределах [{minValue};{maxValue}]:");
                }
            }
        }

        public static Random GetOrCreateRandom()
        {
            if (rnd == null)
            {
                DateTime currentTime = DateTime.UtcNow;
                long unixTimeMilliseconds = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();
                int seed = (int)(unixTimeMilliseconds % (1000 * 60 * 60 * 24));
                rnd = new Random(seed);
            }
            return rnd;
        }


    }
}
