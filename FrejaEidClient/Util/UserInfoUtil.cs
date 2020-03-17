using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Util
{
    internal static class UserInfoUtil
    {
        internal static string ConvertSsnUserInfo(SsnUserInfo ssnUserInfo)
        {
            JsonService jsonService = new JsonService();
            try
            {
                string jsonSsnUserInfo = jsonService.SerializeToJson(ssnUserInfo);
                var jsonSsnUserInfoBytes = System.Text.Encoding.UTF8.GetBytes(jsonSsnUserInfo);
                return System.Convert.ToBase64String(jsonSsnUserInfoBytes);
            }
            catch (Exception ex)
            {
                throw new FrejaEidClientInternalException(message: "Failed to serialize user info.", cause: ex);
            }
        }
    }
}
