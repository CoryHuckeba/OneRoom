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
        public List<FileEntity> files;

        public Directory (string name, string path, Directory parent=null, List<Directory> dirs=null, List<FileEntity> files = null)
        {
            if (parent == null)
                this.parent = this;
            else
                this.parent = parent;

            this.name = name;
            this.path = path;
            if (dirs != null)
                this.directories.InsertRange(0, dirs);
            else
                this.directories = new List<Directory>();
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

        public string PrintSelf()
        {
            string s = "Name: " + this.name + ", Path: " + this.path + ", Contents: ";
            foreach (Directory f in this.directories)
                s = s + f.name + ", ";

            return s;
        }
    }
}
