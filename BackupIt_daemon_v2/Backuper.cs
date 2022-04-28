using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2
{
    public class Backuper
    {
        private void Backup(string destinationPath, string leftPath, string rightPath)
        {
            CompareNodes compare = new();
            Metadata metadata = new(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\metadata.json");

            List<FileSystemNode> leftNodes = compare.JsonToList(leftPath);
            List<FileSystemNode> rightNodes = compare.JsonToList(rightPath);

            List<FileSystemNode> added  = compare.CompareAdded(leftNodes, rightNodes);
            List<FileSystemNode> removed = compare.CompareRemoved(leftNodes, rightNodes);
            List<FileSystemNode> modified = compare.CompareModified(leftNodes, rightNodes);

            //First create all folders (all nodes with IsDirectory = true)
            //then create all files (all nodes with IsDirectory = false)

            foreach (FileSystemNode node in added) //Folders have to be created before files are copied. (maybe?)
            {
                if (node.IsDirectory)
                {
                    Directory.CreateDirectory(Path.Combine(destinationPath, node.FullPath));
                }
            }

            foreach (FileSystemNode node in added)
            {
                if (!node.IsDirectory)
                {
                    File.Copy(node.FullPath, Path.Combine(destinationPath, node.FullPath));
                }
            }

            foreach (FileSystemNode node in modified)
            {
                if (!node.IsDirectory) //Directories do not matter.
                {
                    File.Copy(node.FullPath, Path.Combine(destinationPath, node.FullPath));
                }
            }

            metadata.AddMetadata(added, "added"); //TODO: Remove static input. Maybe get the name of the list.
            metadata.AddMetadata(modified, "modified"); //TODO: Remove static input.
            metadata.AddMetadata(removed, "removed"); //TODO: Remove static input.

        }

    }
}
