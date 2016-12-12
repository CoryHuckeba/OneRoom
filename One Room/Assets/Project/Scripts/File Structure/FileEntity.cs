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
        private const string FILE_EXT = ".drn";

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

            CommandFile newFile = new CommandFile(filename + FILE_EXT, this.path + filename + FILE_EXT, new List<string[]>());
            files.Add(newFile);
            return newFile;
        }

        public bool ContainsFile(string filename)
        {
            foreach (CommandFile f in files)
            {
                if (f.name == filename)
                {
                    return true;
                }
            }

            return false;
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
        private const string HELP_TEXT = "// Welcome to Command Edit!\n" +
            "//   - Double slashes indicate a comment. The compiler ignores everything in a line following them\n" +
            "//   - The following are commands you may give the drone on a run:\n" +
            "//\n" +
            "//      - move [int] - Moves the drone the specified number of meters in the direction it's facing\n" +
            "//      - turn ['left' 'right' or 'around'] - Turns the drone to face the specified direction\n" +
            "//      - grab - Picks up any grabbable object within a meter of the drone\n" +
            "//      - drop - Drops any item held my the drone\n" +
            "//      - open [int] - Opens a door the drone is facing. Pass an optional 4 digit number to use a keycode\n" +
            "//      - scan - Scans the room for useful objects and information. More accurate when closer to stuff.\n" +
            "//      - push - Pushes an object the drone is facing by 1 meter in the same direction\n";
             

        public string name;
        public string path;

        public bool valid = false;

        public string content;
        public List<string[]> commands;

        public event System.Action<bool> validityChange;

        public CommandFile(string name, string path, List<string[]> commands)
        {
            this.name = name;
            this.path = path;
            this.commands = commands;
            this.content = HELP_TEXT;
        }

        public void SetValid(bool valid)
        {
            this.valid = valid;
            if (validityChange != null)
                validityChange(valid);
        }
    }
}
