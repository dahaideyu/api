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
namespace Lkb {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="AreaServiceImplServiceSoapBinding", Namespace="http://service.areaHouse.service.eastcompeace.com/")]
    public partial class AreaServiceImplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback insertAreaOperationCompleted;
        
        private System.Threading.SendOrPostCallback insertAreaExtendOperationCompleted;
        
        /// <remarks/>
        public AreaServiceImplService() {
            this.Url = "http://220.191.208.83:9603/fpps/areaService";
        }
        
        /// <remarks/>
        public event insertAreaCompletedEventHandler insertAreaCompleted;
        
        /// <remarks/>
        public event insertAreaExtendCompletedEventHandler insertAreaExtendCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.areaHouse.service.eastcompeace.com/", ResponseNamespace="http://service.areaHouse.service.eastcompeace.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string insertArea([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("insertArea", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegininsertArea(string arg0, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("insertArea", new object[] {
                        arg0}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndinsertArea(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void insertAreaAsync(string arg0) {
            this.insertAreaAsync(arg0, null);
        }
        
        /// <remarks/>
        public void insertAreaAsync(string arg0, object userState) {
            if ((this.insertAreaOperationCompleted == null)) {
                this.insertAreaOperationCompleted = new System.Threading.SendOrPostCallback(this.OninsertAreaOperationCompleted);
            }
            this.InvokeAsync("insertArea", new object[] {
                        arg0}, this.insertAreaOperationCompleted, userState);
        }
        
        private void OninsertAreaOperationCompleted(object arg) {
            if ((this.insertAreaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.insertAreaCompleted(this, new insertAreaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.areaHouse.service.eastcompeace.com/", ResponseNamespace="http://service.areaHouse.service.eastcompeace.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string insertAreaExtend([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("insertAreaExtend", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegininsertAreaExtend(string arg0, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("insertAreaExtend", new object[] {
                        arg0}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndinsertAreaExtend(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void insertAreaExtendAsync(string arg0) {
            this.insertAreaExtendAsync(arg0, null);
        }
        
        /// <remarks/>
        public void insertAreaExtendAsync(string arg0, object userState) {
            if ((this.insertAreaExtendOperationCompleted == null)) {
                this.insertAreaExtendOperationCompleted = new System.Threading.SendOrPostCallback(this.OninsertAreaExtendOperationCompleted);
            }
            this.InvokeAsync("insertAreaExtend", new object[] {
                        arg0}, this.insertAreaExtendOperationCompleted, userState);
        }
        
        private void OninsertAreaExtendOperationCompleted(object arg) {
            if ((this.insertAreaExtendCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.insertAreaExtendCompleted(this, new insertAreaExtendCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void insertAreaCompletedEventHandler(object sender, insertAreaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class insertAreaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal insertAreaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void insertAreaExtendCompletedEventHandler(object sender, insertAreaExtendCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class insertAreaExtendCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal insertAreaExtendCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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