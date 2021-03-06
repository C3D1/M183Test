﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab1Controller : Controller
    {
        /**
         * 
         * ANTWORTEN BITTE HIER
         * 1.Attack) Man kann den den Parameter "type" im Filepfad abändern.
         * URL: mysite.com/Lab1/Detail?type=../../secretFile.txt
         * Beschreibung: Der Code geht sucht nach dem Verzeichnis und geht alles Files durch. Dadurch hat der Hacker Sicht auf alle Files im Verzeichnis.
         * 2.Attack) Directory traversal attack
         * URL:http://mysite.com/Lab1/Detail?type=../../../../../Windows/system.ini HTTP/1.1
         * Beschreibung:Dies würde das System dazubewegen das system.ini file von windows zu holen und wiedergeben. Mithilfe von "../" geht der Hacker immer ins höhere Verzeichnis. Mit Try and Error gelangt er dann an das File, dass er will. In meinem Beispiel das System.ini file.
         * */


        public ActionResult Index()
        {
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "lions";                
            }
            else
            {
                type.Replace("\\", "");
                type.Replace("/", "");
            }

            var path = "~/Content/images/" + type;

            List<List<string>> fileUriList = new List<List<string>>();

            if (Directory.Exists(Server.MapPath(path)))
            {
                var scheme = Request.Url.Scheme; 
                var host = Request.Url.Host; 
                var port = Request.Url.Port;
                
                string[] fileEntries = Directory.GetFiles(Server.MapPath(path));
                foreach (var filepath in fileEntries)
                {
                    var filename = Path.GetFileName(filepath);
                    var imageuri = scheme + "://" + host + ":" + port + path.Replace("~", "") + "/" + filename;

                    var urilistelement = new List<string>();
                    urilistelement.Add(filename);
                    urilistelement.Add(imageuri);
                    urilistelement.Add(type);

                    fileUriList.Add(urilistelement);
                }
            }
            
            return View(fileUriList);
        }

        public ActionResult Detail()
        {
            var file = Request.QueryString["file"];
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(file))
            {
                file = "Lion1.jpg";
            }
            else
            {
                type.Replace("\\", "");
                type.Replace("/", "");
            }

            if (string.IsNullOrEmpty(type))
            {
                file = "lions";
            }
            else
            {
                type.Replace("\\", "");
                type.Replace("/", "");
            }

            var relpath = "~/Content/images/" + type + "/" + file;

            List<List<string>> fileUriItem = new List<List<string>>();
            var path = Server.MapPath(relpath);

            if (System.IO.File.Exists(path))
            {
                var scheme = Request.Url.Scheme;
                var host = Request.Url.Host;
                var port = Request.Url.Port;
                var absolutepath = Request.Url.AbsolutePath;

                var filename = Path.GetFileName(file);
                var imageuri = scheme + "://" + host + ":" + port + "/Content/images/" + type + "/" + filename;

                var urilistelement = new List<string>();
                urilistelement.Add(filename);
                urilistelement.Add(imageuri);
                urilistelement.Add(type);

                fileUriItem.Add(urilistelement);
            }
            
            return View(fileUriItem);
        }
    }
}