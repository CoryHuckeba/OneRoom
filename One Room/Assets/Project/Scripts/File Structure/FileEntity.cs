using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Files
{

    public abstract class FileEntity
    {
        public string name;
        public string path;
    }

    public class Directory : FileEntity
    {
        public string path;
        public string name;

        public Directory parent;
        public List<Directory> directories;
        public List<CommandFile> files;

        public Directory (string name, string path, Directory parent=null, List<Directory> dirs=null, List<CommandFile> files = null)
        {
            if (parent == null)
                this.parent = this;
            else
                this.parent = parent;

            this.name = name;
            this.path = path;

            // SubDirs
            if (dirs != null)
                this.directories.InsertRange(0, dirs);
            else
                this.directories = new List<Directory>();

            // Files
            if (files != null)
                this.files.InsertRange(0, files);
            else
                this.files = new List<CommandFile>();
        }

        public void AddDirectory(Directory newThing)
        {
            Debug.Log(newThing.PrintSelf());
            this.directories.Add(newThing);
            Debug.Log(PrintSelf());
        }

        public Directory GetDirectory(string name)
        {
            foreach (Directory d in this.directories)
            {
                if (d.name == name)
                    return d;
            }

            // Return null in the fail case
            return null;
        }

        public CommandFile GetOrCreateCommandFile(string filename)
        {
            foreach (CommandFile f in files)
            {
                if (f.name == filename)
                {
                    return f;
                }
            }

            CommandFile newFile = new CommandFile(filename, this.path + filename, new List<string[]>());
            files.Add(newFile);
            return newFile;
        }

        public string PrintSelf()
        {
            string s = "Name: " + this.name + ", Path: " + this.path + ", Contents: ";
            foreach (Directory f in this.directories)
                s = s + f.name + ", ";

            return s;
        }
    }

    public class CommandFile: FileEntity
    {
        public string name;
        public string path;

        public bool valid = false;

        public string content;
        public List<string[]> commands;

        public CommandFile(string name, string path, List<string[]> commands)
        {
            this.name = name;
            this.path = path;
            this.commands = commands;
            this.content = "";
        }
    }
}
