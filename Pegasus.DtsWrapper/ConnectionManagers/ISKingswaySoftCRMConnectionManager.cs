using KingswaySoft.IntegrationToolkit.DynamicsCrm;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pegasus.DtsWrapper.ConnectionManagers
{
    public class ISKingswaySoftCRMConnectionManager : ISConnectionManager
    {
        public ISKingswaySoftCRMConnectionManager(string connectionString, string name, ISProject project = null, ISPackage package = null) : base(connectionString, name, "DynamicsCRM", project, package)
        {

        }
        public ISKingswaySoftCRMConnectionManager(string name, ISProject project = null, ISPackage package = null) : base("", name, "DynamicsCRM", project, package)
        {
        }

        public ISKingswaySoftCRMConnectionManager(ConnectionManager connMgr) : base(connMgr)
        {
        }
        #region Properties

        #region ApiVersion

        public string ApiVersion
        {
            get { return (string)GetConnectionManagerPropertyValue("ApiVersion"); }
            set { SetConnectionManagerPropertyValue("ApiVersion", value); }
        }

        #endregion

        #region AuthorizationServerUrl
        public string AuthorizationServerUrl
        {
            get { return (string)GetConnectionManagerPropertyValue("AuthorizationServerUrl"); }
            set { SetConnectionManagerPropertyValue("AuthorizationServerUrl", value); }
        }

        #endregion

        #region BypassProxyOnLocal

        public bool BypassProxyOnLocal
        {
            get { return (bool)GetConnectionManagerPropertyValue("BypassProxyOnLocal"); }
            set { SetConnectionManagerPropertyValue("BypassProxyOnLocal", value); }
        }

        #endregion

        #region CertificateThumbprint

        public string CertificateThumbprint
        {
            get { return (string)GetConnectionManagerPropertyValue("CertificateThumbprint"); }
            set { SetConnectionManagerPropertyValue("CertificateThumbprint", value); }
        }

        #endregion
        #region ClientAppId

        public string ClientAppId
        {
            get { return (string)GetConnectionManagerPropertyValue("ClientAppId"); }
            set { SetConnectionManagerPropertyValue("ClientAppId", value); }
        }

        #endregion
        #region ClientSecret

        public string ClientSecret
        {
            get { return (string)GetConnectionManagerPropertyValue("ClientSecret"); }
            set { SetConnectionManagerPropertyValue("ClientSecret", value); }
        }

        #endregion
        #region CrmAuthenticationType

        public CrmAuthenticationType CrmAuthenticationType
        {
            get { return (CrmAuthenticationType)GetConnectionManagerPropertyValue("CrmAuthenticationType"); }
            set { SetConnectionManagerPropertyValue("CrmAuthenticationType", value); }
        }

        #endregion
        #region CrmServerUrl

        public string CrmServerUrl
        {
            get { return (string)GetConnectionManagerPropertyValue("CrmServerUrl"); }
            set { SetConnectionManagerPropertyValue("CrmServerUrl", value); }
        }

        #endregion
        #region Domain

        public string Domain
        {
            get { return (string)GetConnectionManagerPropertyValue("Domain"); }
            set { SetConnectionManagerPropertyValue("Domain", value); }
        }

        #endregion
        #region HomeRealmUri

        public string HomeRealmUri
        {
            get { return (string)GetConnectionManagerPropertyValue("HomeRealmUri"); }
            set { SetConnectionManagerPropertyValue("HomeRealmUri", value); }
        }

        #endregion
        #region IgnoreCertificateErrors

        public bool IgnoreCertificateErrors
        {
            get { return (bool)GetConnectionManagerPropertyValue("IgnoreCertificateErrors"); }
            set { SetConnectionManagerPropertyValue("IgnoreCertificateErrors", value); }
        }

        #endregion
        #region OAuthType

        public OAuthType OAuthType
        {
            get { return (OAuthType)GetConnectionManagerPropertyValue("OAuthType"); }
            set { SetConnectionManagerPropertyValue("OAuthType", value); }
        }

        #endregion
        #region Password
        public string Password
        {
            get { return (string)GetConnectionManagerPropertyValue("Password"); }
            set { SetConnectionManagerPropertyValue("Password", value); }
        }

        #endregion
        #region ProxyMode

        public ProxyMode ProxyMode
        {
            get { return (ProxyMode)GetConnectionManagerPropertyValue("ProxyMode"); }
            set { SetConnectionManagerPropertyValue("ProxyMode", value); }
        }

        #endregion
        #region ProxyPassword

        public string ProxyPassword
        {
            get { return (string)GetConnectionManagerPropertyValue("ProxyPassword"); }
            set { SetConnectionManagerPropertyValue("ProxyPassword", value); }
        }

        #endregion
        #region ProxyServer

        public string ProxyServer
        {
            get { return (string)GetConnectionManagerPropertyValue("ProxyServer"); }
            set { SetConnectionManagerPropertyValue("ProxyServer", value); }
        }

        #endregion
        #region ProxyServerPort

        public int ProxyServerPort
        {
            get { return (int)GetConnectionManagerPropertyValue("ProxyServerPort"); }
            set { SetConnectionManagerPropertyValue("ProxyServerPort", value); }
        }

        #endregion
        #region  ProxyUsername

        public string ProxyUsername
        {
            get { return (string)GetConnectionManagerPropertyValue("ProxyUsername"); }
            set { SetConnectionManagerPropertyValue("ProxyUsername", value); }
        }

        #endregion
        #region  RetryOnIntermittentErrors

        public bool RetryOnIntermittentErrors
        {
            get { return (bool)GetConnectionManagerPropertyValue("RetryOnIntermittentErrors"); }
            set { SetConnectionManagerPropertyValue("RetryOnIntermittentErrors", value); }
        }

        #endregion
        #region UserName

        public string UserName
        {
            get { return (string)GetConnectionManagerPropertyValue("UserName"); }
            set { SetConnectionManagerPropertyValue("UserName", value); }
        }

        #endregion
        #region ServiceEndpoint

        public CrmServiceEndpoint ServiceEndpoint
        {
            get { return (CrmServiceEndpoint)GetConnectionManagerPropertyValue("ServiceEndpoint"); }
            set { SetConnectionManagerPropertyValue("ServiceEndpoint", value); }
        }

        #endregion
        #region ServiceResource

        public string ServiceResource
        {
            get { return (string)GetConnectionManagerPropertyValue("ServiceResource"); }
            set { SetConnectionManagerPropertyValue("ServiceResource", value); }
        }

        #endregion
        #region ServiceTimeout

        public uint ServiceTimeout
        {
            get { return (uint)GetConnectionManagerPropertyValue("ServiceTimeout"); }
            set { SetConnectionManagerPropertyValue("ServiceTimeout", value); }
        }

        #endregion
        #endregion
    }
}
