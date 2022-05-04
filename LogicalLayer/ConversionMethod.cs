using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicalLayer
{
    public class ConversionMethod
    {
        public XmlDocument ConvertObjectToXML<T>(T objectToConvert) where T : class
        {
            XmlDocument doc = new XmlDocument();

            Type sourceType = objectToConvert.GetType();

            XmlElement root = doc.CreateElement(sourceType.Name + "s");
            XmlElement rootChild = doc.CreateElement(sourceType.Name);

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            foreach (PropertyInfo pi in sourceProperties)
            {
                if (pi.GetValue(objectToConvert, null) != null)
                {
                    XmlElement child = doc.CreateElement(pi.Name);
                    child.InnerText = Convert.ToString(pi.GetValue(objectToConvert, null));
                    rootChild.AppendChild(child);
                }
            }

            root.AppendChild(rootChild);
            doc.AppendChild(root);

            return doc;

        }

        public List<XmlElement> ConvertObjectToXML<T>(List<T> lstObjectToConvert, XmlDocument xDoc) where T : class
        {
            List<XmlElement> root = new List<XmlElement>();
            if (lstObjectToConvert.Count > 0)
            {

                for (int i = 0; i < lstObjectToConvert.Count; i++)
                {
                    T objectToConvert = lstObjectToConvert[i];

                    XmlElement rootChild = xDoc.CreateElement(lstObjectToConvert[0].GetType().Name);

                    Type sourceType = objectToConvert.GetType();
                    PropertyInfo[] sourceProperties = sourceType.GetProperties();
                    foreach (PropertyInfo pi in sourceProperties)
                    {
                        if (pi.GetValue(objectToConvert, null) != null)
                        {
                            XmlElement child = xDoc.CreateElement(pi.Name);
                            if (pi.ToString().Contains("System.Collections.Generic.List"))
                            {

                                if (pi.GetValue(objectToConvert, null).GetType() == typeof(List<T>))
                                {
                                    List<T> lst = (List<T>)pi.GetValue(objectToConvert, null);
                                    List<XmlElement> rootChild1 = ConvertObjectToXML<T>(lst, xDoc);
                                    if (rootChild1 != null)
                                    {
                                        foreach (XmlElement item in rootChild1)
                                        {
                                            child.AppendChild(item);
                                        }
                                    }
                                }


                            }
                            else
                            {
                                child.InnerText = Convert.ToString(pi.GetValue(objectToConvert, null));

                            }
                            rootChild.AppendChild(child);

                        }
                    }
                    root.Add(rootChild);
                }

            }
            return root;


        }
    }
 
}
