using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koowoo.Services
{
    public class CardNoConvert
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="CardConvertType">
        /// DictID	DictType	DictName
        //254	CardConvertType	8位10进制
        //255	CardConvertType	10位10进制
        //256	CardConvertType	8位8进制
        //257	CardConvertType	10位8进制
        //258	CardConvertType	8位16进制
        //294	CardConvertType	无限制10进制
        /// </param>
        /// <param name="CardType">从表sys_Dict获取ID值  233:IC卡 234：CPU 卡 235身份证</param>
        /// <returns></returns>
        public static string ConvertCardNo(string CardNo, int CardConvertType, int CardType)
        {
            switch (CardConvertType)
            {
                case 254:
                    return Dec8(CardNo, CardType);
                case 255:
                    return "";
                case 256:
                    return "";
                case 257:
                    return "";
                case 258:
                    return "";
                case 294:
                    return ConvertSixteenToTen(CardNo, CardType);
                default:
                    return Dec8(CardNo, CardType);
            }
        }

        private static string ConvertSixteenToTen(string sixteenStr, int CardType)
        {
            if (string.IsNullOrWhiteSpace(sixteenStr))
            {
                return "";
            }
            try
            {
                return Convert.ToUInt32(sixteenStr, 16).ToString();
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 8进制转成10进制0
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="CardType"></param>
        /// <returns></returns>
        private static string Dec8(string CardNo, int CardType)
        {
            int[] Temp = new int[3];
            if (CardType == 233)
            {
                Temp[0] = Convert.ToInt32(CardNo.Substring(4, 2), 16);
                Temp[1] = Convert.ToInt32(CardNo.Substring(2, 2) + CardNo.Substring(0, 2), 16);
                return Temp[0].ToString().PadLeft(3, '0') + Temp[1].ToString().PadLeft(5, '0');
            }
            else if (CardType == 235)
            {
                string idCardNo = CardNo.Substring(CardNo.Length - 6);
                Temp[0] = Convert.ToInt32(idCardNo.Substring(0, 2), 16);
                Temp[1] = Convert.ToInt32(idCardNo.Substring(2, 4), 16);
                return Temp[0].ToString().PadLeft(3, '0') + Temp[1].ToString().PadLeft(5, '0');
            }
            return "";
        }


    }
}
