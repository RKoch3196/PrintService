using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PrintService
{
    class XMLRepository
    {
        public string ReadNode(string nodeName)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("UserConfig.xml");

            XmlNodeList nodelist = xd.SelectNodes("/Configuration");

            foreach (XmlNode node in nodelist)
            {

                return node.SelectSingleNode(nodeName).InnerText;
            }
            return null;
        }
    }
}
