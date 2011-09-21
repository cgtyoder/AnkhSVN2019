﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using Ankh.VSPackage;

namespace Ankh.GenerateVSIXManifest
{
    class Program
    {
        const string vsix2010 = "http://schemas.microsoft.com/developer/vsx-schema/2010";

        static void Main(string[] args)
        {
            using (FileStream fs = File.Create(args[args.Length - 1]))
            using (XmlWriter xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
            {
                xw.WriteStartElement("Vsix", vsix2010);
                xw.WriteAttributeString("Version", "1.0.0");
                AssemblyName asm = new AssemblyName(typeof(Program).Assembly.FullName);
                AssemblyName infoAsm = new AssemblyName(typeof(AnkhSvnPackage).Assembly.FullName);
                xw.WriteComment(string.Format("Generated by {0} {1}", asm.Name, asm.Version));

                xw.WriteStartElement("Identifier", vsix2010);
                xw.WriteAttributeString("Id", string.Format("{0}.{1}.{2}", AnkhId.PlkProduct, AnkhId.PlkVersion, AnkhId.PackageId));

                xw.WriteElementString("Name", AnkhId.ExtensionTitle);
                xw.WriteElementString("Author", AnkhId.AssemblyCompany);
                xw.WriteElementString("Version", infoAsm.Version.ToString());
                xw.WriteElementString("Description", AnkhId.ExtensionDescription);
                xw.WriteElementString("Locale", "1033");
                xw.WriteElementString("MoreInfoUrl", AnkhId.ExtensionMoreInfoUrl);
                xw.WriteElementString("ReleaseNotes", AnkhId.ExtensionReleaseNotesUrl);
                xw.WriteElementString("License", "License.rtf");
                xw.WriteElementString("GettingStartedGuide", AnkhId.ExtensionGettingStartedUrl);
                xw.WriteElementString("Icon", AnkhId.PlkProduct + "-Icon.png");
                xw.WriteElementString("PreviewImage", AnkhId.PlkProduct + "-Preview.png");
                xw.WriteElementString("InstalledByMsi", "true");
                xw.WriteElementString("AllUsers", "true");

                xw.WriteStartElement("SupportedProducts", vsix2010);
                foreach (string version in new string[] { "10.0", "11.0" })
                {
                    xw.WriteStartElement("VisualStudio", vsix2010);
                    xw.WriteAttributeString("Version", version);
                    xw.WriteElementString("Edition", "IntegratedShell");
                    xw.WriteElementString("Edition", "Pro");
                    xw.WriteElementString("Edition", "Premium");
                    xw.WriteElementString("Edition", "Ultimate");
                    xw.WriteElementString("Edition", "Express_All");
                    xw.WriteEndElement();
                }
                xw.WriteEndElement(); // /SupportedProducts

                xw.WriteStartElement("SupportedFrameworkRuntimeEdition", vsix2010);
                xw.WriteAttributeString("MinVersion", "2.0");
                xw.WriteAttributeString("MaxVersion", "4.5");
                xw.WriteEndElement(); // /SupportedFrameworkRuntimeEdition

                xw.WriteEndElement(); // /Identifier

                xw.WriteStartElement("References", vsix2010);
                xw.WriteEndElement();
                xw.WriteStartElement("Content", vsix2010);
                xw.WriteElementString("VsPackage", vsix2010, "Ankh.Package.dll");
                xw.WriteEndElement();

                xw.WriteEndElement(); // /Vsix
            }
        }
    }
}
