using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public class Convert
    {
        /// <summary>
        /// Converts Enum into string. Size of characters does not matter.
        /// </summary>
        /// <typeparam name="T">Type of Enum</typeparam>
        /// <param name="TEnum">Enum value</param>
        /// <returns>String name of Enum variable or null if variable name is NULL</returns>
        public static string ToString<T>(T TEnum) where T : struct, IConvertible
        {
            string sName = Enum.GetName(typeof(T), TEnum);
            if (sName.ToUpper() == "NULL")
                return null;
            else return sName;
        }

        /// <summary>
        /// Converts String into Enum. Size of characters does not matter.
        /// </summary>
        /// <typeparam name="T">Type of Enum</typeparam>
        /// <param name="sName">String name of Enum</param>
        /// <returns>Returns Enum variable or default (0'th) element of enum if no such string exist.</returns>
        public static T ToEnum<T>(string sName) where T : struct, IConvertible
        {
            // if (!typeof(T).IsEnum)
            if (sName == null) return default(T);
            sName = sName.ToUpper();

            foreach (T TEnum in (T[])Enum.GetValues(typeof(T)))
            {
                string sTEName = Convert.ToString<T>(TEnum);
                if (sTEName == null) continue;
                else sTEName = sTEName.ToUpper();
                if (sTEName == sName) return TEnum;
            }

            return default(T);
        }
    }
}
