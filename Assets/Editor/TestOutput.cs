using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace LuaTesting
{
    [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        [XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class testsuites
        {

            private testsuitesTestsuite testsuiteField;

            /// <remarks/>
            public testsuitesTestsuite testsuite
            {
                get
                {
                    return testsuiteField;
                }
                set
                {
                    testsuiteField = value;
                }
            }
        }

        /// <remarks/>
        [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public class testsuitesTestsuite
        {

            private testsuitesTestsuiteProperty[] propertiesField;

            private testsuitesTestsuiteTestcase[] testcaseField;

            private object systemoutField;

            private object systemerrField;

            private string nameField;

            private byte idField;

            private string packageField;

            private string hostnameField;

            private byte testsField;

            private DateTime timestampField;

            private decimal timeField;

            private byte errorsField;

            private byte failuresField;

            private byte skippedField;

            /// <remarks/>
            [XmlArrayItemAttribute("property", IsNullable = false)]
            public testsuitesTestsuiteProperty[] properties
            {
                get
                {
                    return propertiesField;
                }
                set
                {
                    propertiesField = value;
                }
            }

            /// <remarks/>
            [XmlElementAttribute("testcase")]
            public testsuitesTestsuiteTestcase[] testcase
            {
                get
                {
                    return testcaseField;
                }
                set
                {
                    testcaseField = value;
                }
            }

            /// <remarks/>
            [XmlElementAttribute("system-out")]
            public object systemout
            {
                get
                {
                    return systemoutField;
                }
                set
                {
                    systemoutField = value;
                }
            }

            /// <remarks/>
            [XmlElementAttribute("system-err")]
            public object systemerr
            {
                get
                {
                    return systemerrField;
                }
                set
                {
                    systemerrField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string name
            {
                get
                {
                    return nameField;
                }
                set
                {
                    nameField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte id
            {
                get
                {
                    return idField;
                }
                set
                {
                    idField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string package
            {
                get
                {
                    return packageField;
                }
                set
                {
                    packageField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string hostname
            {
                get
                {
                    return hostnameField;
                }
                set
                {
                    hostnameField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte tests
            {
                get
                {
                    return testsField;
                }
                set
                {
                    testsField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public DateTime timestamp
            {
                get
                {
                    return timestampField;
                }
                set
                {
                    timestampField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public decimal time
            {
                get
                {
                    return timeField;
                }
                set
                {
                    timeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte errors
            {
                get
                {
                    return errorsField;
                }
                set
                {
                    errorsField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte failures
            {
                get
                {
                    return failuresField;
                }
                set
                {
                    failuresField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte skipped
            {
                get
                {
                    return skippedField;
                }
                set
                {
                    skippedField = value;
                }
            }
        }

        /// <remarks/>
        [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public class testsuitesTestsuiteProperty
        {

            private string nameField;

            private string valueField;

            /// <remarks/>
            [XmlAttributeAttribute]
            public string name
            {
                get
                {
                    return nameField;
                }
                set
                {
                    nameField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string value
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }
        }

        /// <remarks/>
        [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public class testsuitesTestsuiteTestcase
        {

            private testsuitesTestsuiteTestcaseError errorField;

            private testsuitesTestsuiteTestcaseFailure failureField;

            private string classnameField;

            private string nameField;

            private decimal timeField;

            /// <remarks/>
            public testsuitesTestsuiteTestcaseError error
            {
                get
                {
                    return errorField;
                }
                set
                {
                    errorField = value;
                }
            }

            /// <remarks/>
            public testsuitesTestsuiteTestcaseFailure failure
            {
                get
                {
                    return failureField;
                }
                set
                {
                    failureField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string classname
            {
                get
                {
                    return classnameField;
                }
                set
                {
                    classnameField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string name
            {
                get
                {
                    return nameField;
                }
                set
                {
                    nameField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public decimal time
            {
                get
                {
                    return timeField;
                }
                set
                {
                    timeField = value;
                }
            }
        }

        /// <remarks/>
        [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public class testsuitesTestsuiteTestcaseError
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [XmlAttributeAttribute]
            public string type
            {
                get
                {
                    return typeField;
                }
                set
                {
                    typeField = value;
                }
            }

            /// <remarks/>
            [XmlTextAttribute]
            public string Value
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }
        }

        /// <remarks/>
        [SerializableAttribute]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public class testsuitesTestsuiteTestcaseFailure
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [XmlAttributeAttribute]
            public string type
            {
                get
                {
                    return typeField;
                }
                set
                {
                    typeField = value;
                }
            }

            /// <remarks/>
            [XmlTextAttribute]
            public string Value
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }
        }
}