﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码由 wsdl 自动生成, Version=4.6.1055.0。
// 
namespace Lkb
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DeviceAlarmServiceImplServiceSoapBinding", Namespace="http://service.device.service.eastcompeace.com/")]
    public partial class DeviceAlarmServiceImplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback insertDeviceAlarmOperationCompleted;
        
        /// <remarks/>
        public DeviceAlarmServiceImplService() {
            this.Url = "http://220.191.208.83:9603/fpps/devicealarmService";
        }
        
        /// <remarks/>
        public event insertDeviceAlarmCompletedEventHandler insertDeviceAlarmCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.device.service.eastcompeace.com/", ResponseNamespace="http://service.device.service.eastcompeace.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string insertDeviceAlarm([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("insertDeviceAlarm", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegininsertDeviceAlarm(string arg0, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("insertDeviceAlarm", new object[] {
                        arg0}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndinsertDeviceAlarm(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void insertDeviceAlarmAsync(string arg0) {
            this.insertDeviceAlarmAsync(arg0, null);
        }
        
        /// <remarks/>
        public void insertDeviceAlarmAsync(string arg0, object userState) {
            if ((this.insertDeviceAlarmOperationCompleted == null)) {
                this.insertDeviceAlarmOperationCompleted = new System.Threading.SendOrPostCallback(this.OninsertDeviceAlarmOperationCompleted);
            }
            this.InvokeAsync("insertDeviceAlarm", new object[] {
                        arg0}, this.insertDeviceAlarmOperationCompleted, userState);
        }
        
        private void OninsertDeviceAlarmOperationCompleted(object arg) {
            if ((this.insertDeviceAlarmCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.insertDeviceAlarmCompleted(this, new insertDeviceAlarmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void insertDeviceAlarmCompletedEventHandler(object sender, insertDeviceAlarmCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class insertDeviceAlarmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal insertDeviceAlarmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}
