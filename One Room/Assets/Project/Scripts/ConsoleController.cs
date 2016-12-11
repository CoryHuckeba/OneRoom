using Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public delegate void CommandHandler(string[] args);

public class ConsoleController {

    #region Event Declarations

    public delegate void LogChangedHandler(string[] log);
    public event LogChangedHandler logChanged;

    public delegate void CommandLogChangedHandler(string[] log);
    public event CommandLogChangedHandler commandLogChanged;

    public delegate void VisibilityChangedHandler(bool visible);
    public event VisibilityChangedHandler visibilityChanged;

    public delegate void WorkingPathChangedHandler(string newPath);
    public event WorkingPathChangedHandler workingPathChanged;

    public delegate void OpenEditorHandler(bool opening, CommandFile file);
    public event OpenEditorHandler openEditor;

    #endregion Event Declarations


    #region Helper Classes

    class CommandRegistration 
    {
        public string command { get; private set; }
        public CommandHandler handler { get; private set; }
        public string help { get; private set; }

        public CommandRegistration(string command, CommandHandler handler, string help)
        {
            this.command = command;
            this.handler = handler;
            this.help = help;
        }
    }

    #endregion Helper Classes


    #region Properties & Variables

    public Directory currentDirectory;

    const int scrollbackSize = 100;
    Queue<string> scrollback = new Queue<string>(scrollbackSize);
    Queue<string> commandScrollback = new Queue<string>(scrollbackSize);

    List<string> commandHistory = new List<string>();
    Dictionary<string, CommandRegistration> commands = new Dictionary<string, CommandRegistration>();

    // Line formatting
    const int COMMAND_WIDTH = 17;
    const int TEXT_WIDTH = 51;
    const int PATH_WIDTH = 15;

    // Help Formatting
    const string DRONE_HELP = "Commands for configuring and running drone operations\n" +
        "\tset <slot number> <file name>: Sets the provided file to the specified drone config slot\n" + 
        "\tclear <slot number> OR 'all': Removes the file from the provided slot number\n" +
        "\trun: Runs all provided files in order of slot number";

    const string COMMAND_EDIT_HELP = "Commands for creation and editing drone batch files\n" +
        "\t";

    public string[] log { get; private set; } // Copy of scrollback as an array for easier use by ConsoleView
    public string[] commandLog { get; private set; } // Copy of scrollback as an array for easier use by ConsoleView

    const string repeatCmdName = "!!"; // Name of the repeat command, constant since it needs to skip these if they are in the command history

    #endregion Properties & Variables


    #region Console Management

    public ConsoleController()
    {
        //When adding commands, you must add a call below to registerCommand() with its name, implementation method, and help text.
        registerCommand("drone_set", setSlot, "takes a slot number and a command file name in the present directory.");
        registerCommand("drone_clear", clearSlot, "takes a slot number or 'all' and clears the passed slot (or all slots)");
        registerCommand("help", help, "Print this help.");
        registerCommand("hide", hide, "Hide the console.");
        registerCommand(repeatCmdName, repeatCommand, "Repeat last command.");
        registerCommand("resetprefs", resetPrefs, "Reset & saves PlayerPrefs.");
        registerCommand("mkdir", makeDirectory, "Create new directory");
        registerCommand("ls", listDir, "Lists contents of current directory");
        registerCommand("cd", changeDirectory, "Changes the current directory to the specified one");
        registerCommand("cmd_edit", commandEdit, COMMAND_EDIT_HELP);
    }

    private void registerCommand(string command, CommandHandler handler, string help)
    {
        commands.Add(command, new CommandRegistration(command, handler, help));
    }

    public void appendCommandLine(string line)
    {
        // Shoprten the command to fit within the 17 character limit
        if (line.Length > COMMAND_WIDTH)
        {
            line = line.Substring(0, 14) + "...";
        }

        if (commandScrollback.Count >= scrollbackSize)
        {
            commandScrollback.Dequeue();
        }
        commandScrollback.Enqueue(line);

        commandLog = commandScrollback.ToArray();
        if (commandLogChanged != null)
        {
            commandLogChanged(commandLog);
        }
    }

    public void appendLogLine(string line)
    {
        Debug.Log(line);

        // If the log line is longer than the text area in console, move the command line up as well
        if (line.Length > TEXT_WIDTH)
        {
            appendCommandLine("");
        }

        if (scrollback.Count >= ConsoleController.scrollbackSize)
        {
            scrollback.Dequeue();
        }
        scrollback.Enqueue(line);

        log = scrollback.ToArray();
        if (logChanged != null)
        {
            logChanged(log);
        }
    }

    public void runCommandString(string commandString)
    {
        appendCommandLine("$ " + commandString);

        string[] commandSplit = parseArguments(commandString);
        string[] args = new string[0];
        if (commandSplit.Length < 1)
        {
            appendLogLine(string.Format("Unable to process command '{0}'", commandString));
            return;

        }
        else if (commandSplit.Length >= 2)
        {
            int numArgs = commandSplit.Length - 1;
            args = new string[numArgs];
            Array.Copy(commandSplit, 1, args, 0, numArgs);
        }
        runCommand(commandSplit[0].ToLower(), args);
        commandHistory.Add(commandString);
    }

    public void runCommand(string command, string[] args)
    {
        CommandRegistration reg = null;
        if (!commands.TryGetValue(command, out reg))
        {
            appendLogLine(string.Format("Unknown command '{0}', type 'help' for list.", command));
        }
        else
        {
            if (reg.handler == null)
            {
                appendLogLine(string.Format("Unable to process command '{0}', handler was null.", command));
            }
            else
            {
                reg.handler(args);
            }
        }
    }

    static string[] parseArguments(string commandString)
    {
        LinkedList<char> parmChars = new LinkedList<char>(commandString.ToCharArray());
        bool inQuote = false;
        var node = parmChars.First;
        while (node != null)
        {
            var next = node.Next;
            if (node.Value == '"')
            {
                inQuote = !inQuote;
                parmChars.Remove(node);
            }
            if (!inQuote && node.Value == ' ')
            {
                node.Value = '\n';
            }
            node = next;
        }
        char[] parmCharsArr = new char[parmChars.Count];
        parmChars.CopyTo(parmCharsArr, 0);
        return (new string(parmCharsArr)).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    }

    #endregion Console Management


    #region Console Command Handlers

    void changeDirectory(string[] args)
    {
        // Error Handling
        if (args.Length < 1)
        {
            appendLogLine("Expected an argument.");
            return;
        }
        if (args.Length > 1)
        {
            appendLogLine("Missing directory name");
            return;
        }

        Directory target;
        if (args[0] == "..")
            target = currentDirectory.parent;
        else
            target = currentDirectory.GetDirectory(args[0]);

        if (target == null)
        {
            appendLogLine("'" + args[0] + "' is not a valid directory.");
            return;
        }
        else
        {
            if (target.path.Length > PATH_WIDTH)
            {
                this.workingPathChanged("..." + target.path.Substring(target.path.Length - 12));
            }
            else
                this.workingPathChanged(target.path);

            currentDirectory = target;
            appendLogLine("");
        }
    }

    void makeDirectory(string[] args)
    {
        // Error Handling
        if (args.Length < 1)
        {
            appendLogLine("Expected an argument.");
            return;
        }
        if (args.Length > 1)
        {
            appendLogLine("Missing directory name");
            return;
        }

        string dirName = args[0];

        // Ensure directory does not already exist
        if(currentDirectory.GetDirectory(dirName) != null)
        {
            appendLogLine("Directory '" + dirName + "' already exists.");
            return;
        }

        Directory dir = new Directory(dirName, currentDirectory.path + dirName + "/", currentDirectory);
        currentDirectory.AddDirectory(dir);
        appendLogLine("\n");
    }

    // TODO: Make this print files too
    void listDir(string[] args)
    {
        if (args.Length > 0)
        {
            appendLogLine("command takes no arguments");
            return;
        }
        if (currentDirectory.directories.Count == 0)
        {
            appendLogLine("current directory is empty");
            return;
        }

        // Format string of directory contents
        string content = "";
        foreach (Directory d in currentDirectory.directories)
        {
            content += (d.name + "\t");
        }
        foreach (CommandFile f in currentDirectory.files)
        {
            content += (f.name + "\t");
        }
        appendLogLine(content);
    }

    void setSlot(string[] args)
    {
        if (args.Length < 2)
        {
            appendLogLine("Expected two parameters: <integer - slot number> and <string - file name>");
            return;
        }

        // Check slot number arg
        int slotNum = 0;
        if(!int.TryParse(args[0], out slotNum))
        {
            appendLogLine("Unable to parse slot number '" + args[0] + "'. Expected integer.");
            return;
        }
        else if (slotNum > 7 || slotNum < 1)
        {
            appendLogLine("Invalid slot number '" + args[0] + "'. Expected integer from 1-7.");
            return;
        }

        // Check filename arg
        if (!currentDirectory.ContainsFile(args[1]))
        {
            appendLogLine("Invalid file name '" + args[1] + "'. Please specify a file in the present directory.");
            return;
        }

        CommandFile file = currentDirectory.GetOrCreateCommandFile(args[1]);
        SlotManager.Instance.SetFile(slotNum, file);
    }

    void clearSlot(string[] args)
    {
        if (args.Length != 1)
        {
            appendLogLine("Invalid number of arguments, please pass a slot number or the string 'all'.");
            return;
        }
        int slotNum = -1;
        if (!int.TryParse(args[0], out slotNum) && args[0] != "all")
        {
            appendLogLine("Unable to parse argument '" + args[0]  + "'. Please pass a slot number or the string 'all'.");
            return;
        }
        if (args[0] == "all")
            SlotManager.Instance.clearFile();
        else
            SlotManager.Instance.clearFile(slotNum);
        appendLogLine("");
    }

    void help(string[] args)
    {
        foreach (CommandRegistration reg in commands.Values)
        {
            appendLogLine(string.Format("{0}: {1}", reg.command, reg.help));
        }
    }

    void hide(string[] args)
    {
        if (visibilityChanged != null)
        {
            visibilityChanged(false);
        }
    }

    void repeatCommand(string[] args)
    {
        for (int cmdIdx = commandHistory.Count - 1; cmdIdx >= 0; --cmdIdx)
        {
            string cmd = commandHistory[cmdIdx];
            if (String.Equals(repeatCmdName, cmd))
            {
                continue;
            }
            runCommandString(cmd);
            break;
        }
    }

    void commandEdit(string[] args)
    {
        if (args.Length != 1)
        {
            appendLogLine("Expected a filename to open or create");
            return;
        }

        CommandFile file = currentDirectory.GetOrCreateCommandFile(args[0]);
        appendLogLine("");
        this.openEditor(true, file);
    }

    void resetPrefs(string[] args)
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    #endregion Console Command Handlers



    #region MonoBehaviour Implementation

    // Use this for initialization
    void Start () {
		// Needed?
	}
	
	// Update is called once per frame
	void Update () {
        // Needed?
    }

    #endregion MonoBehaviour Implementation


    #region Public Interface


    #endregion Public Interface
}
