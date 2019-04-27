using Silverlake.Api.Controllers;
using Silverlake.Api.Resources.Constants;
using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Silverlake.Api.Common
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        #region Send method.
        /// <summary>   
        /// Send method.   
        /// </summary>   
        /// <param name="request">Request parameter</param>   
        /// <param name="cancellationToken">Cancellation token parameter</param>   
        /// <returns>Return HTTP response.</returns>   
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string customMessage = "";
            try
            {
                // Initialization.   
                IEnumerable<string> apiKeyHeaderValues = null;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;
                //string userName = null;
                //string password = null;
                // Verification.   
                if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out apiKeyHeaderValues) && !string.IsNullOrEmpty(authorization.Parameter))
                {
                    var apiKeyHeaderValue = apiKeyHeaderValues.First();
                    // Get the auth token   
                    string authToken = authorization.Parameter;
                    // Decode the token from BASE64   
                    string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    // Extract username and password from decoded token   
                    //userName = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                    //password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

                    //string encryptedPassword = CustomEncryptorDecryptor.EncryptPassword(password);
                    StringBuilder filter = new StringBuilder();
                    filter.Append(" 1=1");
                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(User.ApiAuthToken)) + " = '" + apiKeyHeaderValue + "'");
                    //filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(User.Status)) + " = '1'");
                    List<User> userMatches = IUserService.GetDataByFilter(filter.ToString(), 0, 0, false);
                    if (userMatches.Count > 0)
                    {
                        User user = userMatches.FirstOrDefault();
                        if (user.Status == 1)
                        {
                            if (user.BranchId != 0)
                            {
                                Branch branch = IBranchService.GetSingle(user.BranchId);
                                if (branch != null)
                                {
                                    if (branch.Status == 1)
                                    {
                                        ConfigurationController configurationController = new ConfigurationController();
                                        ConfigurationDTO config = (ConfigurationDTO)configurationController.Get(decodedToken);
                                        if (config.responseMsg == "Branch")
                                        {
                                            // Setting   
                                            var identity = new GenericIdentity(decodedToken);
                                            SetPrincipal(new GenericPrincipal(identity, null));
                                            //response = request.CreateResponse(HttpStatusCode.OK);
                                            customMessage = "success";
                                        }
                                        else
                                        {
                                            customMessage = config.responseMsg;
                                            response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                                            response.ReasonPhrase = config.responseMsg;
                                        }
                                    }
                                    else
                                    {
                                        customMessage = "Branch not active!";
                                        response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                                        response.ReasonPhrase = customMessage;
                                    }
                                }
                                else
                                {
                                    customMessage = "No Branch!";
                                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                                    response.ReasonPhrase = customMessage;
                                }
                            }
                            else
                            {
                                customMessage = "Not Branch user!";
                                response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                                response.ReasonPhrase = customMessage;
                            }
                        }
                        else
                        {
                            customMessage = "User not active!";
                            response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                            response.ReasonPhrase = customMessage;
                            //var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "User not active!", Content = new StringContent("User not active!") };
                            //throw new HttpResponseException(msg);
                        }
                    }
                    else
                    {
                        customMessage = "User doesn't exist!";
                        response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                        response.ReasonPhrase = customMessage;
                        //var msg = nesw HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "User doesn't exist!" };
                        //throw new HttpResponseException(msg);
                    }
                    // Verification.   
                    //if (apiKeyHeaderValue.Equals(ApiInfo.API_KEY_VALUE) && userName.Equals(ApiInfo.USERNAME_VALUE) && password.Equals(ApiInfo.PASSWORD_VALUE))
                    //{

                    //}
                }
                // Info.   
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("Exception: " + ex.Message);
                customMessage = "Exception: " + ex.Message;
                response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                response.ReasonPhrase = customMessage;
            }
            if (customMessage == "success" || customMessage == "")
            {
                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(task =>
                {
                    return response;
                });
            }
        }
        #endregion

        #region Set principal method.  
        /// <summary>   
        /// Set principal method.   
        /// </summary>   
        /// <param name="principal">Principal parameter</param>   
        private static void SetPrincipal(IPrincipal principal)
        {
            // setting.   
            Thread.CurrentPrincipal = principal;
            // Verification.   
            if (HttpContext.Current != null)
            {
                // Setting.   
                HttpContext.Current.User = principal;
            }
        }
        #endregion
    }
}