﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

// 
// Этот исходный код был создан с помощью xsd, версия=4.6.81.0.
// 


namespace AisBenefits.Infrastructure.Eip
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.6.81.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:ekburg.ru:eip:common:AsyncService:v1.0")]
    [XmlRoot("AsyncResults", Namespace = "urn:ekburg.ru:eip:common:AsyncService:v1.0", IsNullable = false)]
    public partial class AsyncResultsType
    {

        private AsyncResultType[] asyncResultField;

        /// <remarks/>
        [XmlElement("AsyncResult")]
        public AsyncResultType[] AsyncResult
        {
            get
            {
                return this.asyncResultField;
            }
            set
            {
                this.asyncResultField = value;
            }
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.6.81.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:ekburg.ru:eip:common:AsyncService:v1.0")]
    public class AsyncResultType
    {

        private string requestIdField;

        private DateTime acceptedDateTimeField;

        private AsyncRequestStateType stateField;

        private string statusCodeField;

        private string statusDescriptionField;

        /// <remarks/>
        [XmlElement(DataType = "token")]
        public string RequestId
        {
            get
            {
                return requestIdField;
            }
            set
            {
                requestIdField = value;
            }
        }

        /// <remarks/>
        public DateTime AcceptedDateTime
        {
            get
            {
                return acceptedDateTimeField;
            }
            set
            {
                acceptedDateTimeField = value;
            }
        }

        /// <remarks/>
        public AsyncRequestStateType State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlElement(DataType = "token")]
        public string StatusCode
        {
            get
            {
                return statusCodeField;
            }
            set
            {
                statusCodeField = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return statusDescriptionField;
            }
            set
            {
                statusDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.6.81.0")]
    [Serializable]
    [XmlType(Namespace = "urn:ekburg.ru:eip:common:AsyncService:v1.0")]
    public enum AsyncRequestStateType
    {

        /// <remarks/>
        Processing,

        /// <remarks/>
        Completed
    }
}