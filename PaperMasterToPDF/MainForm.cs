using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PaperMasterToPDF
{
    public partial class MainForm : Form
    {
        struct PMHeader
        {
            public int signature;
            public int signature2;
            public byte[] unknown;
            public short unknown2;
            public short numFields;
        }

        struct PMField
        {
            public string name;
            public byte type; // 2 means string where len contains the 3 of bytes or 0 for a variable length string, 6 means 32 bit value
            public byte len;
            public short offset;
        }

        class PMDocument
        {
            private UInt32 m_docID; // The unique ID for this document
            private UInt32 m_conID; // The unique ID for the folder that contains this document
            private string m_name; // The document's name

            public PMDocument(UInt32 docID, UInt32 conID, string name)
            {
                m_docID = docID;
                m_conID = conID;
                m_name = name;
            }

            public UInt32 ConID { get { return m_conID; } }

            public UInt32 DocID { get { return m_docID; } }

            public string Name {  get { return m_name; } }

            public override string ToString()
            {
                return m_name;
            }
        }

        struct PMFolder
        {
            public UInt32 conID; // The unique ID for this folder
            public string hostPath; // The physical path in the PM Cabinet for this folder
            public string pfcPath; // The PaperMaster Cabinet Path for this folder
        }

        struct PMFile
        {
            public UInt16 type;
            public string subPath;
            public UInt32 order;
            public UInt32 rpos;
            public UInt32 rsize;
            public UInt32 createTime;
            public UInt32 browserFlags;
            public string name;
        }

        struct PMPage
        {
            public UInt16 type;
            public string location; // Filename of this page
            public string name; // Page #
        }

        private string CabinetFolder = "R:\\PMLive_Cabinets\\ATARI8DO";
        private string OutputFolder = "R:\\Junk";
        private List<PMDocument> lstDocuments;
        private SortedList<UInt32, PMFolder> lstFolders;
        private SortedList<string, SortedList<string, PMFile>> filesCache;

        public MainForm()
        {
            InitializeComponent();

            SelectPaperMasterCabinetDialog.SelectedPath = CabinetFolder;
            CabinetFolderLabel.Text = SelectPaperMasterCabinetDialog.SelectedPath;

            SelectOutputFolderDialog.SelectedPath = OutputFolder;
            OutputFolderLabel.Text = SelectOutputFolderDialog.SelectedPath;

            List<PMDocument> lstDocuments = new List<PMDocument>();
            List<PMFolder> lstFolders = new List<PMFolder>();
            filesCache = new SortedList<string, SortedList<string, PMFile>>();
        }

        private void OpenCabinetButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = SelectPaperMasterCabinetDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string PMCabinetRoot = SelectPaperMasterCabinetDialog.SelectedPath;

                string docIDFilename = PMCabinetRoot + "\\_DOCID._PS";
                lstDocuments = ReadDocuments(docIDFilename);

                string folderIDFilename = PMCabinetRoot + "\\_foldid._ps";
                lstFolders = ReadFolders(folderIDFilename);

                DocumentsListBox.Items.Clear();
                foreach (PMDocument document in lstDocuments)
                {
                    DocumentsListBox.Items.Add(document);
                }

                CabinetFolder = PMCabinetRoot;
                CabinetFolderLabel.Text = PMCabinetRoot;

                filesCache = new SortedList<string, SortedList<string, PMFile>>();
            }
        }

        private void SelectOutputFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = SelectOutputFolderDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                OutputFolder = SelectOutputFolderDialog.SelectedPath;
                OutputFolderLabel.Text = OutputFolder;
            }
        }

        PMHeader ReadPMHeader(FileStream fs)
        {
            PMHeader header = new PMHeader();

            BinaryReader br = new BinaryReader(fs);

            header.signature = br.ReadInt32();
            header.signature2 = br.ReadInt32();
            header.unknown = br.ReadBytes(52);
            header.unknown2 = br.ReadInt16();
            header.numFields = br.ReadInt16();

            return header;
        }

        PMField ReadPMField(FileStream fs)
        {
            PMField field = new PMField();

            BinaryReader br = new BinaryReader(fs);

            byte[] nameBytes = br.ReadBytes(28);
            field.name = AsciiBytesToString(nameBytes, 28);
            field.type = br.ReadByte();
            field.len = br.ReadByte();
            field.offset = br.ReadInt16();

            return field;
        }

        List<PMDocument> ReadDocuments(string docFilename)
        {
            List<PMDocument> lstDocuments = null;

            FileStream fs = new FileStream(docFilename, FileMode.Open);
            if (fs.CanRead)
            {
                // Read Header
                PMHeader header = ReadPMHeader(fs);

                // Validate header signature as PaperMaster
                if (header.signature != 5457140)
                {
                    ;
                }

                // Read fields
                List<PMField> lstFields = new List<PMField>();
                for (short fieldNum = 0; fieldNum < header.numFields; ++fieldNum)
                {
                    PMField field = ReadPMField(fs);
                    lstFields.Add(field);
                }

                lstDocuments = new List<PMDocument>();

                // Read document entries
                while (fs.Position < fs.Length) // While not End of File
                {
                    //PMDocument pmDoc = new PMDocument();
                    UInt32 docID = 0;
                    UInt32 conID = 0;
                    string name = "Error - Unassigned Name";

                    BinaryReader br = new BinaryReader(fs);

                    short recordlen = br.ReadInt16();
                    int offset = br.ReadInt32();

                    foreach (PMField field in lstFields)
                    {
                        switch (field.name)
                        {
                            case "PFC_DOC_ID":
                                docID = br.ReadUInt32();
                                break;
                            case "PFC_CON_ID":
                                conID = br.ReadUInt32();
                                break;
                            case "NAME":
                                name = ReadString(br, field.len);
                                break;
                        }
                    }

                    lstDocuments.Add(new PMDocument(docID, conID, name));
                }

                fs.Close();
            }

            return lstDocuments;
        }

        SortedList<UInt32, PMFolder> ReadFolders(string folderFilename)
        {
            SortedList<UInt32, PMFolder> lstFolders = null;

            FileStream fs = new FileStream(folderFilename, FileMode.Open);
            if (fs.CanRead)
            {
                // Read Header
                PMHeader header = ReadPMHeader(fs);

                // Validate header signature as PaperMaster
                if (header.signature != 5457140)
                {
                    ;
                }

                // Read fields
                List<PMField> lstFields = new List<PMField>();
                for (short fieldNum = 0; fieldNum < header.numFields; ++fieldNum)
                {
                    PMField field = ReadPMField(fs);
                    lstFields.Add(field);
                }

                lstFolders = new SortedList<UInt32, PMFolder>();

                // Read document entries
                while (fs.Position < fs.Length) // While not End of File
                {
                    PMFolder pmFolder = new PMFolder();

                    BinaryReader br = new BinaryReader(fs);

                    short recordlen = br.ReadInt16();
                    int offset = br.ReadInt32();

                    foreach (PMField field in lstFields)
                    {
                        switch (field.name)
                        {
                            case "PFC_CON_ID":
                                pmFolder.conID = br.ReadUInt32();
                                break;
                            case "HOST_PATH":
                                pmFolder.hostPath = ReadString(br, field.len);
                                break;
                            case "PFC_PATH":
                                pmFolder.pfcPath = ReadString(br, field.len);
                                break;
                        }
                    }

                    lstFolders.Add(pmFolder.conID, pmFolder);
                }

                fs.Close();
            }

            return lstFolders;
        }

        SortedList<string, PMFile> ReadFiles(string filesFilename)
        {
            SortedList<string, PMFile> lstFiles = null;

            // Check for DirectoryNotFoundException
            FileStream fs = new FileStream(filesFilename, FileMode.Open);
            if (fs.CanRead)
            {
                // Read Header
                PMHeader header = ReadPMHeader(fs);

                // Validate header signature as PaperMaster
                if (header.signature != 5457140)
                {
                    ;
                }

                // Read fields
                List<PMField> lstFields = new List<PMField>();
                for (short fieldNum = 0; fieldNum < header.numFields; ++fieldNum)
                {
                    PMField field = ReadPMField(fs);
                    lstFields.Add(field);
                }

                lstFiles = new SortedList<string, PMFile>();

                // Read document entries
                while (fs.Position < fs.Length) // While not End of File
                {
                    PMFile pmFile = new PMFile();

                    BinaryReader br = new BinaryReader(fs);

                    short recordlen = br.ReadInt16();
                    int offset = br.ReadInt32();

                    foreach (PMField field in lstFields)
                    {
                        switch (field.name)
                        {
                            case "TYPE":
                                pmFile.type = br.ReadUInt16();
                                break;
                            case "LOCATION":
                                pmFile.subPath = ReadString(br, field.len);
                                break;
                            case "ORDER":
                                pmFile.order = br.ReadUInt32();
                                break;
                            case "RPOS":
                                pmFile.rpos = br.ReadUInt32();
                                break;
                            case "RSIZE":
                                pmFile.rsize = br.ReadUInt32();
                                break;
                            case "PFC_CREATE_TIME":
                                pmFile.createTime = br.ReadUInt32();
                                break;
                            case "PFC_BROWSER_FLAGS":
                                pmFile.browserFlags = br.ReadUInt32();
                                break;
                            case "NAME":
                                pmFile.name = ReadString(br, field.len);
                                break;
                        }
                    }

                    lstFiles.Add(pmFile.name, pmFile);
                }

                fs.Close();
            }

            return lstFiles;
        }
        List<PMPage> ReadPages(string pagesFilename)
        {
            List<PMPage> lstPages = null;

            FileStream fs = new FileStream(pagesFilename, FileMode.Open);
            if (fs.CanRead)
            {
                // Read Header
                PMHeader header = ReadPMHeader(fs);

                // Validate header signature as PaperMaster
                if (header.signature != 5457140)
                {
                    ;
                }

                // Read fields
                List<PMField> lstFields = new List<PMField>();
                for (short fieldNum = 0; fieldNum < header.numFields; ++fieldNum)
                {
                    PMField field = ReadPMField(fs);
                    lstFields.Add(field);
                }

                lstPages = new List<PMPage>();

                // Read document entries
                while (fs.Position < fs.Length) // While not End of File
                {
                    PMPage pmPage = new PMPage();

                    BinaryReader br = new BinaryReader(fs);

                    short recordlen = br.ReadInt16();
                    int offset = br.ReadInt32();

                    foreach (PMField field in lstFields)
                    {
                        switch (field.name)
                        {
                            case "TYPE":
                                pmPage.type = br.ReadUInt16();
                                break;
                            case "LOCATION":
                                pmPage.location = ReadString(br, field.len);
                                break;
                            case "NAME":
                                pmPage.name = ReadString(br, field.len);
                                break;
                        }
                    }

                    lstPages.Add(pmPage);
                }

                fs.Close();
            }

            return lstPages;
        }

        public string AsciiBytesToString(byte[] buffer, int maxLength)
        {
            for (int i = 0; i < maxLength; i++)
            {
                /// If we find a NULL terminator then return the string to that point
                if (buffer[i] == 0)
                {
                    return Encoding.ASCII.GetString(buffer, 0, i);
                }
            }

            /// NULL terminator not found so the entire buffer is the string.
            return Encoding.ASCII.GetString(buffer, 0, maxLength);
        }

        public string ReadString(BinaryReader br, short len)
        {
            if (len == 0)
            {
                len = br.ReadInt16();
                len -= 2;
            }
            byte[] nameBytes = br.ReadBytes(len);
            string result = AsciiBytesToString(nameBytes, len);

            return result;
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            CheckDocuments(true);
        }

        private void DeselectAll_Click(object sender, EventArgs e)
        {
            CheckDocuments(false);
        }

        private void CheckDocuments(bool check)
        {
            for (int i = 0; i < DocumentsListBox.Items.Count; i++)
            {
                DocumentsListBox.SetItemChecked(i, check);
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            // Process each selected/checked document
            for (int i = 0; i < DocumentsListBox.Items.Count; i++)
            {
                if (DocumentsListBox.GetItemChecked(i))
                {
                    PMDocument document = (PMDocument)DocumentsListBox.Items[i];

                    if (!lstFolders.ContainsKey(document.ConID))
                    {
                        break;
                    }
                    PMFolder folder = lstFolders[document.ConID];

                    // Get the 'sub directory' that contains this document and find it in the subdirectory
                    string folderPath = SelectPaperMasterCabinetDialog.SelectedPath + "\\" + folder.hostPath;
                    string filePath = folderPath + "\\_PFC._PS";

                    // Check if we've already read and cached the files list or we need to read it and add it to our cache
                    SortedList<string, PMFile> lstFiles;
                    if (filesCache.ContainsKey(filePath))
                    {
                        lstFiles = filesCache[filePath];
                    }
                    else
                    {
                        lstFiles = ReadFiles(filePath);
                        filesCache.Add(filePath, lstFiles);
                    }

                    if (!lstFiles.ContainsKey(document.Name))
                    {
                        break;
                    }
                    PMFile file = lstFiles[document.Name];

                    // Get a list of TIFF pages for this document
                    folderPath = folderPath + "\\" + file.subPath;
                    filePath = folderPath + "\\_PFC._PS";
                    List<PMPage> lstPages = ReadPages(filePath);

                    // Create a new PDF document
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.Info.Title = "Generated with PaperMasterToPDF using PDFsharp";

                    // Add each TIFF image to the PDF document
                    foreach (PMPage page in lstPages)
                    {
                        string pageFilename = folderPath + "\\" + page.location;

                        // Create an empty page
                        PdfPage pdfPage = pdfDocument.AddPage();

                        // Get an XGraphics object for drawing
                        XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                        XImage tiffImage = XImage.FromFile(pageFilename);

                        if (tiffImage.PixelHeight > tiffImage.PixelWidth)
                        {
                            // Reader as portrait image
                            pdfPage.Orientation = PageOrientation.Portrait;
                        }
                        else
                        {
                            // Reader as landscape image
                            pdfPage.Orientation = PageOrientation.Landscape;
                        }

                        double width = tiffImage.PixelWidth * 72 / tiffImage.HorizontalResolution;
                        double height = tiffImage.PixelHeight * 72 / tiffImage.HorizontalResolution;

                        gfx.DrawImage(tiffImage, 0, 0, width, height);
                    }

                    // Generate output filename
                    string documentName = document.Name.Replace(':', '-').Replace('\\', '_').Replace('/', '_');
                    string pdfFilename = String.Format("{0}\\{1}.PDF", OutputFolder, documentName);
                    pdfDocument.Save(pdfFilename);

                    // If Preview is checked then immediately display the converted file
                    if (PreviewCheckBox.Checked)
                    {
                        System.Diagnostics.Process.Start(pdfFilename);
                    }
                }
            }
        }
    }
}
