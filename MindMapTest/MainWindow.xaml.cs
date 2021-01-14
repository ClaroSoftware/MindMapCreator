using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xaml;
using System.Xml;

namespace MindMapTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //Strings to hold specified node values
        public string centerNodeContent { get; set; }
        public string childNodeContent { get; set; }
        public ObservableCollection<string> childNodes { get; set; } = new ObservableCollection<string>();


        public MainWindow()
        {
            InitializeComponent();
        }
        
        //Path to this application
        static string application_Dir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";

        //Paths to generated files
        string mm_File = application_Dir + "Created_MM_File.mm";
        string mv_File = application_Dir + "Created_MV_File.xml";
        string txt_file = application_Dir + "Created_TXT_File.txt";
        string mmap_file = application_Dir + "Created_MMap_File.mmap";
        string xmind_file = application_Dir + "Created_Xmind_File.xmind";
        string cid_File = application_Dir + "Created_Cid_File.cid";

        //Paths to Applications
        string ClaroIdeas_Path = "C:\\Program Files (x86)\\Claro Software\\ClaroIdeas\\ClaroIdeas.exe";
        string MindView_Path = "C:\\Program Files\\MatchWare\\MindView 8.0\\MindView.exe";
        string Inspiration_Path = "C:\\Program Files (x86)\\Inspiration 10 IE\\Insp10IE.exe";
        string IdeaMapper_Path = "C:\\Program Files (x86)\\IdeaMapper4Students\\IdeaMapper4Students.exe";
        string Xmind_Path = "C:\\Program Files\\XMind\\XMind.exe";
        string Freemind_Path = "C:\\Program Files (x86)\\FreeMind\\FreeMind.exe";
        string MindManager_Path = "C:\\Program Files\\MindManager 21\\MindManager.exe";


        #region Create mind map buttons

        private void ClaroIdeas_Click(object sender, RoutedEventArgs e)
        {
            createCidfile();
            OpenWithCmd(cid_File, ClaroIdeas_Path);
        }

        private void MindView_Click(object sender, RoutedEventArgs e)
        {
            createMVfile();
            OpenWithCmd(mv_File, MindView_Path);
        }
        
        //TODO: Opening .isf files, .txt files allow for the creation of nodes in Inspiration graphs but not edges 
        private void Inspiration_Click(object sender, RoutedEventArgs e)
        {
            createTxtFile();
            OpenWithCmd(txt_file, Inspiration_Path);
        }
        
        private void Ideamapper_Click(object sender, RoutedEventArgs e)
        {
            createMMapfile();
            OpenWithCmd(mmap_file, IdeaMapper_Path);
        }

        private void Xmind_Click(object sender, RoutedEventArgs e)
        {
            createXmindfile();
            OpenWithCmd(xmind_file, Xmind_Path);
        }
        
        private void Freemind_Click(object sender, RoutedEventArgs e)
        {
            createMMfile();
            OpenWithCmd(mm_File, Freemind_Path);
        }

        private void MindManager_Click(object sender, RoutedEventArgs e)
        {
            createMMapfile();
            OpenWithCmd(mmap_file, MindManager_Path);
        }

        #endregion

        #region Creating .mm files

        private List<string> getMMLines()
        {
            List<string> lines = new List<string>();

            lines.Add("<?xml version=\"1.0\" encoding=\"UTF - 8\"?>");
            lines.Add("<map version=\"0.9.0_Beta_8\">");
            lines.Add("<!-- To view this file, download free mind mapping software FreeMind from http://freemind.sourceforge.net -->");

            long Created_num = 1176612313201;
            long Id_num = 1916449201;
            long Modified_num = 1176615919578;

            //Creating center node
            lines.Add("<node CREATED =\"" + Created_num.ToString() + "\" ID=\"Freemind_Link_" + Id_num.ToString() + "\" MODIFIED=\"" + Modified_num.ToString() + "\" TEXT=\"" + centerNodeContent + "\">");
            Created_num += 1;
            Id_num += 1;
            Modified_num += 1;

            //Creating child nodes
            foreach(string node in childNodes)
            {
                lines.Add("<node CREATED =\"" + Created_num.ToString() + "\" FOLDED=\"false\" ID=\"Freemind_Link_" + Id_num.ToString() + "\" MODIFIED=\"" + Modified_num.ToString() + "\" POSITION=\"right\" TEXT=\"" + node + "\"/>");
                Created_num += 1;
                Id_num += 1;
                Modified_num += 1;
            }

            lines.Add("</node>");
            lines.Add("</map>");

            return lines;
        }

        private void createMMfile()
        {
            string filePath = application_Dir + "Created_MM_File.mm";

            if (File.Exists(filePath))
                File.Delete(filePath);

            List<string> lines = getMMLines();

            File.WriteAllLines(filePath, lines.ToArray());
        }

        #endregion

        #region Creating MindView files

        private void createMVfile()
        {
            string filePath = application_Dir + "Created_MV_File.xml";

            if (File.Exists(filePath))
                File.Delete(filePath);

            List<string> lines = getMVLines();

            File.WriteAllLines(filePath, lines.ToArray());
        }

        private List<string> getMVLines()
        {
            List<string> lines = new List<string>();

            int id = 1;

            lines.Add("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            lines.Add("<mwmv:MindView xmlns:mwmv=\"http://schema.matchware.com/mindview\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://schema.matchware.com/mindview http://schema.matchware.com/mindview\" WordWrapText=\"1\" HorizontalGap=\"20\" MinimumBranchLength=\"100\" Time=\"ProjectManagement\">");
            lines.Add("<mwmv:Roots>");

            //Creating center node
            lines.Add("<mwmv:Branch Id=\""+ id.ToString() + "\" Color=\"4280690214\" BackgroundColor=\"#262626\" LineColor=\"#939AA4\" PercentComplete=\"0\">");
            lines.Add("<mwmv:BranchText Color=\"#FFFFFF\" Font=\"Arial,18,1\">"+ centerNodeContent +"</mwmv:BranchText>");
            lines.Add("<mwmv:SubBranches>");


            //Creating child nodes
            foreach (string node in childNodes)
            {
                id++;
                lines.Add("<mwmv:Branch Id=\""+id.ToString()+"\" Color=\"4289049533\" BackgroundColor=\"#E2E8EF\" LineColor=\"#939AA4\" PercentComplete=\"0\">");
                lines.Add("<mwmv:BranchText Font=\"Arial, 14, 1\">"+node+"</mwmv:BranchText>");
                lines.Add("</mwmv:Branch>");
            }

            lines.Add("</mwmv:SubBranches>");
            lines.Add("</mwmv:Branch>");
            lines.Add("</mwmv:Roots>");
            lines.Add("</mwmv:MindView>");

            return lines;
        }

        #endregion

        #region Creating .txt files
        private List<string> getTxtLines()
        {
            List<string> lines = new List<string>();
            lines.Add(centerNodeContent);

            foreach (string node in childNodes)
                lines.Add("\n" + node + "\0");

            return lines;
        }

        private void createTxtFile()
        {
            string filePath = application_Dir + "Created_TXT_File.txt";

            if (File.Exists(filePath))
                File.Delete(filePath);

            List<string> lines = getTxtLines();

            File.WriteAllLines(filePath, lines.ToArray());
        }

        #endregion

        #region Creating .mmap files

        private List<string> getMMapLines()
        {
            List<string> lines = new List<string>();

            lines.Add("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            lines.Add("<?xml-client name=\"MindManager\" version=\"21.0.261\" platform=\"Windows\"?>");
            lines.Add("<ap:Map xmlns:ap=\"http://schemas.mindjet.com/MindManager/Application/2003\" xmlns:cor=\"http://schemas.mindjet.com/MindManager/Core/2003\" xmlns:pri=\"http://schemas.mindjet.com/MindManager/Primitive/2003\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Dirty=\"0000000000000001\" OId=\"XckaCc/A0EKL3Jj/zm3DJw==\" Gen=\"0000000000000000\" xsi:schemaLocation=\"http://schemas.mindjet.com/MindManager/Application/2003 http://schemas.mindjet.com/MindManager/Application/2003 http://schemas.mindjet.com/MindManager/Core/2003 http://schemas.mindjet.com/MindManager/Core/2003 http://schemas.mindjet.com/MindManager/Delta/2003 http://schemas.mindjet.com/MindManager/Delta/2003 http://schemas.mindjet.com/MindManager/Primitive/2003 http://schemas.mindjet.com/MindManager/Primitive/2003\">");
            lines.Add("<cor:Custom xmlns:cst0=\"http://schemas.mindjet.com/MindManager/UpdateCompatibility/2004\" Index=\"0\" Uri=\"http://schemas.mindjet.com/MindManager/UpdateCompatibility/2004\" Dirty=\"0000000000000001\" cst0:UpdatedCategories=\"true\" cst0:UpdatedTextLabelSetIds=\"true\" cst0:UpdatedVisibilityStyle=\"true\" cst0:UpdatedDuration=\"true\" cst0:UpdatedBusinessTasks=\"true\" cst0:UpdatedGanttViewProperties=\"true\" cst0:UpdatedNamedView=\"true\" cst0:UpdatedMainTopicsOrder=\"true\" cst0:UpdatedTopicLineWeights=\"true\" cst0:UpdatedSmartFillToConditionalFormatting=\"true\" cst0:UpdatedTaskHighlightingToConditionalFormatting=\"true\" cst0:UpdatedSlidesFormat=\"true\" />");
            lines.Add("<ap:OneTopic>");
            lines.Add("<ap:Topic Dirty=\"0000000000000001\" OId=\""+ Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "\" Gen=\"0000000000000000\">");
            lines.Add("<cor:Custom xmlns:cst1=\"http://schemas.iaresearch.com/PTMAddin\" Index=\"0\" Uri=\"http://schemas.iaresearch.com/PTMAddin\" Dirty=\"0000000000000001\" cst1:CurrencySymbol=\"£\" cst1:Calendar=\"0111110\" cst1:WorkHours=\"8.000000\" cst1:HighlightRollupOnly=\"0\" cst1:CurrencyDecimalPlaces=\"2\" cst1:CurrencyPositiveFormat=\"1\" cst1:CurrencyNegativeFormat=\"2\" />");
            lines.Add("<ap:SubTopics>");
            
            //Creating child nodes
            foreach (string node in childNodes)
            {
                string iod = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //Generate unique OID
                lines.Add("<ap:Topic Dirty=\"0000000000000001\" OId=\""+ iod + "\" Gen=\"0000000000000000\">");
                lines.Add("<ap:TopicViewGroup ViewIndex=\"0\"/>");
                lines.Add("<ap:Text Dirty=\"0000000000000001\" PlainText=\""+ node +"\" ReadOnly=\"false\">");
                lines.Add("<ap:Font />");
                lines.Add("</ap:Text>");
                lines.Add("<ap:Offset Dirty=\"0000000000000001\" CX=\"2.\" CY=\"0.\" />");
                lines.Add("</ap:Topic>");
            }
            
            lines.Add("</ap:SubTopics>");
            lines.Add("<ap:TopicViewGroup ViewIndex=\"0\">");
            lines.Add("<ap:Collapsed Collapsed=\"false\" Dirty=\"0000000000000000\" />");
            lines.Add("</ap:TopicViewGroup>");
            lines.Add("<ap:Text Dirty=\"0000000000000001\" PlainText=\""+centerNodeContent+"\" ReadOnly=\"false\">"); //Create center node
            lines.Add("<ap:Font />");
            lines.Add("</ap:Text>");
            lines.Add("</ap:Topic>");
            lines.Add("</ap:OneTopic>");
            
            //Adding Styles
            lines.Add("<ap:StyleGroup>");
            lines.Add("<ap:RootTopicDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"ffecf4fa\" LineColor=\"ff3283c0\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Center\" TextCapitalization=\"urn:mindjet:SentenceStyle\" VerticalTextAlignment=\"urn:mindjet:Top\" Dirty=\"0000000000000000\" PlainText=\"Central Topic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"14.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:RoundedRectangle\" Dirty=\"0000000000000000\" LeftMargin=\"4.\" RightMargin=\"4.\" TopMargin=\"4.\" BottomMargin=\"4.\" VerticalLeftMargin=\"4.\" VerticalRightMargin=\"4.\" VerticalTopMargin=\"4.\" VerticalBottomMargin=\"4.\" VerticalSubTopicShape=\"urn:mindjet:RoundedRectangle\" />");
            lines.Add("<ap:DefaultLabelFloatingTopicShape LabelFloatingTopicShape=\"urn:mindjet:None\" Dirty=\"0000000000000000\" LeftMargin=\"0.\" RightMargin=\"0.\" TopMargin=\"0.\" BottomMargin=\"0.\" VerticalLeftMargin=\"2.5\" VerticalRightMargin=\"2.5\" VerticalTopMargin=\"2.5\" VerticalBottomMargin=\"2.5\" VerticalLabelFloatingTopicShape=\"urn:mindjet:None\" />");
            lines.Add("<ap:DefaultCalloutFloatingTopicShape CalloutFloatingTopicShape=\"urn:mindjet:None\" Dirty=\"0000000000000000\" LeftMargin=\"0.\" RightMargin=\"0.\" TopMargin=\"0.\" BottomMargin=\"0.\" VerticalLeftMargin=\"2.5\" VerticalRightMargin=\"2.5\" VerticalTopMargin=\"2.5\" VerticalBottomMargin=\"2.5\" VerticalCalloutFloatingTopicShape=\"urn:mindjet:None\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" TopicWidthControl=\"urn:mindjet:AutoWidth\" Dirty=\"0000000000000000\" Width=\"80.\" MinimumHeight=\"5.\" Padding=\"2.\" IsUsingFixedSize=\"false\" FixedWidth=\"75.\" FixedHeight=\"25.\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:Right\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsShapeWidthFactor=\"1.\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:UpAndDown\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsDepth=\"1\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Bottom\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("<ap:DefaultImageThumbnailSize ThumbnailSize=\"20.\" Dirty=\"0000000000000000\" MaxFullSize=\"300.\" />");
            lines.Add("</ap:RootTopicDefaultsGroup>");
            lines.Add("<ap:RootSubTopicDefaultsGroup Level=\"0\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"ffecf4fa\" LineColor=\"ff3283c0\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Main Topic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"12.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"0.5\" RightMargin=\"0.5\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"0.5\" VerticalRightMargin=\"0.5\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalSubTopicShape=\"urn:mindjet:Capsule\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" Dirty=\"0000000000000000\" Width=\"150.\" Padding=\"2.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Bottom\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:RootSubTopicDefaultsGroup>");
            lines.Add("<ap:RootSubTopicDefaultsGroup Level=\"1\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00000000\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Subtopic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"10.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Line\" Dirty=\"0000000000000000\" LeftMargin=\"0.800000011920928955078125\" RightMargin=\"0.800000011920928955078125\" TopMargin=\"0.800000011920928955078125\" BottomMargin=\"0.800000011920928955078125\" VerticalLeftMargin=\"0.800000011920928955078125\" VerticalRightMargin=\"0.800000011920928955078125\" VerticalTopMargin=\"0.800000011920928955078125\" VerticalBottomMargin=\"0.800000011920928955078125\" VerticalSubTopicShape=\"urn:mindjet:Line\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" Dirty=\"0000000000000000\" Width=\"60.700000762939453125\" Padding=\"1.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Bottom\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:RootSubTopicDefaultsGroup>");
            lines.Add("<ap:CalloutTopicDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"fffbd1bc\" LineColor=\"ffcf4d0c\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Callout\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"10.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultCalloutFloatingTopicShape CalloutFloatingTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"0.5\" RightMargin=\"0.5\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"0.5\" VerticalRightMargin=\"0.5\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalCalloutFloatingTopicShape=\"urn:mindjet:Capsule\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" Dirty=\"0000000000000000\" Width=\"61.\" Padding=\"2.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Center\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:CalloutTopicDefaultsGroup>");
            lines.Add("<ap:CalloutSubTopicDefaultsGroup Level=\"0\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00000000\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Subtopic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"10.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Line\" Dirty=\"0000000000000000\" LeftMargin=\"0.800000011920928955078125\" RightMargin=\"0.800000011920928955078125\" TopMargin=\"0.800000011920928955078125\" BottomMargin=\"0.800000011920928955078125\" VerticalLeftMargin=\"0.800000011920928955078125\" VerticalRightMargin=\"0.800000011920928955078125\" VerticalTopMargin=\"0.800000011920928955078125\" VerticalBottomMargin=\"0.800000011920928955078125\" VerticalSubTopicShape=\"urn:mindjet:Line\" />");
            lines.Add("<ap:DefaultTopicLayout Dirty=\"0000000000000000\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Center\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:CalloutSubTopicDefaultsGroup>");
            lines.Add("<ap:LabelTopicDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"ffecf4fa\" LineColor=\"ff3283c0\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Floating Topic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"12.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultLabelFloatingTopicShape LabelFloatingTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"0.5\" RightMargin=\"0.5\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"0.5\" VerticalRightMargin=\"0.5\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalLabelFloatingTopicShape=\"urn:mindjet:Capsule\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" Dirty=\"0000000000000000\" Width=\"90.\" Padding=\"2.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:Down\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsDepth=\"1\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Center\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:LabelTopicDefaultsGroup>");
            lines.Add("<ap:LabelSubTopicDefaultsGroup Level=\"0\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00000000\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"Subtopic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"10.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Line\" Dirty=\"0000000000000000\" LeftMargin=\"0.800000011920928955078125\" RightMargin=\"0.800000011920928955078125\" TopMargin=\"0.800000011920928955078125\" BottomMargin=\"0.800000011920928955078125\" VerticalLeftMargin=\"0.800000011920928955078125\" VerticalRightMargin=\"0.800000011920928955078125\" VerticalTopMargin=\"0.800000011920928955078125\" VerticalBottomMargin=\"0.800000011920928955078125\" VerticalSubTopicShape=\"urn:mindjet:Line\" />");
            lines.Add("<ap:DefaultTopicLayout Dirty=\"0000000000000000\" Width=\"61.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Bottom\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:LabelSubTopicDefaultsGroup>");
            lines.Add("<ap:OrgChartTopicDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"ffecf4fa\" LineColor=\"ff3283c0\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" VerticalTextAlignment=\"urn:mindjet:Top\" Dirty=\"0000000000000000\" PlainText=\"Org - Chart Topic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"12.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"0.5\" RightMargin=\"0.5\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"0.5\" VerticalRightMargin=\"0.5\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalSubTopicShape=\"urn:mindjet:Capsule\" />");
            lines.Add("<ap:DefaultLabelFloatingTopicShape LabelFloatingTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"2.\" RightMargin=\"2.\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"5.\" VerticalRightMargin=\"5.\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalLabelFloatingTopicShape=\"urn:mindjet:RoundedRectangle\" />");
            lines.Add("<ap:DefaultCalloutFloatingTopicShape CalloutFloatingTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"2.\" RightMargin=\"2.\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"5.\" VerticalRightMargin=\"5.\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalCalloutFloatingTopicShape=\"urn:mindjet:RoundedRectangleBalloon\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" TopicWidthControl=\"urn:mindjet:AutoWidth\" Dirty=\"0000000000000000\" Width=\"150.\" MinimumHeight=\"5.\" Padding=\"2.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Center\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Vertical\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsShapeWidthFactor=\"1.\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsDepth=\"1\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Center\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:OrgChartTopicDefaultsGroup>");
            lines.Add("<ap:OrgChartSubTopicDefaultsGroup Level=\"0\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00000000\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" VerticalTextAlignment=\"urn:mindjet:Top\" Dirty=\"0000000000000000\" PlainText=\"Subtopic\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"10.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Line\" Dirty=\"0000000000000000\" LeftMargin=\"0.800000011920928955078125\" RightMargin=\"0.800000011920928955078125\" TopMargin=\"0.800000011920928955078125\" BottomMargin=\"0.800000011920928955078125\" VerticalLeftMargin=\"0.800000011920928955078125\" VerticalRightMargin=\"0.800000011920928955078125\" VerticalTopMargin=\"0.800000011920928955078125\" VerticalBottomMargin=\"0.800000011920928955078125\" VerticalSubTopicShape=\"urn:mindjet:Line\" />");
            lines.Add("<ap:DefaultCalloutFloatingTopicShape Dirty=\"0000000000000000\" VerticalLeftMargin=\"2.\" VerticalRightMargin=\"2.\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalCalloutFloatingTopicShape=\"urn:mindjet:RectangleBalloon\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" TopicWidthControl=\"urn:mindjet:AutoWidth\" Dirty=\"0000000000000000\" Width=\"60.700000762939453125\" MinimumHeight=\"5.\" Padding=\"1.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Bottom\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsShapeWidthFactor=\"1.\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsDepth=\"1\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Center\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:OrgChartSubTopicDefaultsGroup>");
            lines.Add("<ap:RelationshipDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00000000\" LineColor=\"ffcf4d0c\" />");
            lines.Add("<ap:DefaultLineStyle LineDashStyle=\"urn:mindjet:Solid\" Dirty=\"0000000000000000\" LineWidth=\"1.5\" />");
            lines.Add("<ap:DefaultConnectionStyle ConnectionShape=\"urn:mindjet:NoArrow\" Index=\"0\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:DefaultConnectionStyle ConnectionShape=\"urn:mindjet:Arrow\" Index=\"1\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:DefaultRelationshipLineShape LineShape=\"urn:mindjet:Bezier\" Dirty=\"0000000000000000\" />");
            lines.Add("</ap:RelationshipDefaultsGroup>");
            lines.Add("<ap:BoundaryDefaultsGroup>");
            lines.Add("<ap:DefaultLineStyle LineDashStyle=\"urn:mindjet:Solid\" Dirty=\"0000000000000000\" LineWidth=\"1.5\" />");
            lines.Add("<ap:DefaultBoundaryShape BoundaryShape=\"urn:mindjet:CurvedLine\" Dirty=\"0000000000000000\" Margin=\"0.\" />");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"fffbd1bc\" LineColor=\"ffcf4d0c\" />");
            lines.Add("</ap:BoundaryDefaultsGroup>");
            lines.Add("<ap:Structure StructureGrowthDirection=\"urn:mindjet:Automatic\" Dirty=\"0000000000000000\" UseAutoLayout=\"true\" MinimumMainTopicsHeight=\"12.\" FadeNotSelectedObjects=\"true\" UseCurveAntialiasing=\"true\" UseTextAntialiasing=\"true\" MainTopicLineWidth=\"0.529166698455810546875\" VerticalMainTopicLineWidth=\"0.529166698455810546875\" SiblingSpacing=\"0.\" ParentChildSpacing=\"0.\" UseOrganicLines=\"false\" HideCollapseSign=\"false\" UseLineWidthForAllTopics=\"true\" />");
            lines.Add("<ap:BackgroundFill Dirty=\"0000000000000000\" FillColor=\"ffffffff\" />");
            lines.Add("<ap:NotesDefaultFont Size=\"10.\" Name=\"Verdana\" Color=\"ff000000\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:TimelineSubTopicDefaultsGroup Level=\"0\">");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"ffecf4fa\" LineColor=\"ff3283c0\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Left\" TextCapitalization=\"urn:mindjet:None\" Dirty=\"0000000000000000\" PlainText=\"\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"12.\" Name=\"Segoe UI\" Color=\"ff000000\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultSubTopicShape SubTopicShape=\"urn:mindjet:Capsule\" Dirty=\"0000000000000000\" LeftMargin=\"0.5\" RightMargin=\"0.5\" TopMargin=\"2.\" BottomMargin=\"2.\" VerticalLeftMargin=\"0.5\" VerticalRightMargin=\"0.5\" VerticalTopMargin=\"2.\" VerticalBottomMargin=\"2.\" VerticalSubTopicShape=\"urn:mindjet:Capsule\" />");
            lines.Add("<ap:DefaultTopicLayout TopicLayoutHorizontalAlignment=\"urn:mindjet:Center\" TopicLayoutVerticalAlignment=\"urn:mindjet:Center\" TopicTextAndImagePosition=\"urn:mindjet:TextRightImageLeft\" Dirty=\"0000000000000000\" Width=\"150.\" Padding=\"2.\" IsUsingFixedSize=\"false\" IsTidy=\"false\" />");
            lines.Add("<ap:DefaultSubTopicsShape SubTopicsAlignment=\"urn:mindjet:Bottom\" SubTopicsConnectionPoint=\"urn:mindjet:Outside\" SubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" SubTopicsGrowth=\"urn:mindjet:Horizontal\" SubTopicsGrowthDirection=\"urn:mindjet:AutomaticHorizontal\" SubTopicsShape=\"urn:mindjet:Vertical\" SubTopicsVerticalAlignment=\"urn:mindjet:Middle\" SubTopicsVerticalGrowthDirection=\"urn:mindjet:AutomaticVertical\" Dirty=\"0000000000000000\" DistanceFromParent=\"2.\" VerticalDistanceFromParent=\"2.\" DistanceBetweenSiblings=\"2.\" VerticalDistanceBetweenSiblings=\"2.\" SubTopicsAlignmentDualVertical=\"urn:mindjet:Bottom\" SubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" VerticalSubTopicsConnectionStyle=\"urn:mindjet:RoundedElbow\" VerticalSubTopicsConnectionPoint=\"urn:mindjet:Outside\" VerticalSubTopicsTreeConnectionPoint=\"urn:mindjet:Inside\" />");
            lines.Add("<ap:DefaultSubTopicsVisibility Dirty=\"0000000000000000\" Hidden=\"false\" />");
            lines.Add("</ap:TimelineSubTopicDefaultsGroup>");
            lines.Add("<ap:BackgroundObjectDefaultsGroup>");
            lines.Add("<ap:DefaultColor Dirty=\"0000000000000000\" FillColor=\"00ffffff\" LineColor=\"ff444444\" />");
            lines.Add("<ap:DefaultLineStyle LineDashStyle=\"urn:mindjet:Solid\" Dirty=\"0000000000000000\" LineWidth=\"1.\" />");
            lines.Add("<ap:DefaultBackgroundObjectType BackgroundObjectType=\"urn:mindjet:Rectangle\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:DefaultText TextAlignment=\"urn:mindjet:Center\" TextCapitalization=\"urn:mindjet:None\" VerticalTextAlignment=\"urn:mindjet:Top\" Dirty=\"0000000000000001\" PlainText=\"Text Label\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"14.\" Name=\"Segoe UI\" Color=\"ff444444\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultText>");
            lines.Add("<ap:DefaultRectangleSize Dirty=\"0000000000000000\" Width=\"211.6699981689453125\" Height=\"105.8300018310546875\" />");
            lines.Add("<ap:DefaultCircleSize Dirty=\"0000000000000000\" Width=\"105.8300018310546875\" Height=\"105.8300018310546875\" />");
            lines.Add("<ap:DefaultHorizontalDividerSize Dirty=\"0000000000000000\" Width=\"211.6699981689453125\" Height=\"0.\" />");
            lines.Add("<ap:DefaultVerticalDividerSize Dirty=\"0000000000000000\" Width=\"0.\" Height=\"211.6699981689453125\" />");
            lines.Add("<ap:DefaultTextSize Dirty=\"0000000000000000\" Width=\"31.75\" Height=\"9.2600002288818359375\" />");
            lines.Add("<ap:DefaultHorizontalSwimlanesSize Dirty=\"0000000000000000\" Width=\"211.6699981689453125\" Height=\"105.8300018310546875\" />");
            lines.Add("<ap:DefaultVerticalSwimlanesSize Dirty=\"0000000000000000\" Width=\"105.8300018310546875\" Height=\"211.6699981689453125\" />");
            lines.Add("<ap:DefaultHorizontalSwimlaneCount SwimlaneCount=\"2\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:DefaultVerticalSwimlaneCount SwimlaneCount=\"2\" Dirty=\"0000000000000000\" />");
            lines.Add("<ap:DefaultFillet Dirty=\"0000000000000000\" Radius=\"1.7999999523162841796875\" />");
            lines.Add("<ap:DefaultTitleText TextAlignment=\"urn:mindjet:Center\" TextCapitalization=\"urn:mindjet:None\" VerticalTextAlignment=\"urn:mindjet:Top\" Dirty=\"0000000000000000\" PlainText=\"\" ReadOnly=\"false\">");
            lines.Add("<ap:Font Size=\"14.\" Name=\"Segoe UI\" Color=\"ff444444\" Bold=\"false\" Italic=\"false\" Underline=\"false\" Strikethrough=\"false\" />");
            lines.Add("</ap:DefaultTitleText>");
            lines.Add("</ap:BackgroundObjectDefaultsGroup>");
            lines.Add("<ap:AppliedStyleName Name=\"\" Dirty=\"0000000000000000\" />");
            lines.Add("</ap:StyleGroup>");
            //End of styles

            lines.Add("</ap:Map>");

            return lines;
        }
        
        private void createMMapfile()
        {
            string fileCreation_DirectoryPath = application_Dir + "mmap_creator\\";
            string mapConfig_FilePath = fileCreation_DirectoryPath + "Document.xml";
            string zip_FilePath = application_Dir + "Created_MMap_File.zip";
            string output_FilePath = application_Dir + "Created_MMap_File.mmap";

            if (File.Exists(mapConfig_FilePath))
                File.Delete(mapConfig_FilePath);

            List<string> lines = getMMapLines();
            File.WriteAllLines(mapConfig_FilePath, lines.ToArray());

            if (File.Exists(output_FilePath))
                File.Delete(output_FilePath);

            if (File.Exists(zip_FilePath))
                File.Delete(zip_FilePath);

            ZipFile.CreateFromDirectory(fileCreation_DirectoryPath, zip_FilePath);
            File.Copy(zip_FilePath, output_FilePath);

        }

        #endregion

        #region Creating .xmind files

        private List<string> getXmindLines()
        {
            List<string> lines = new List<string>();
            lines.Add("[");
            lines.Add("{");
            lines.Add("\"id\": \"faf9405ffeea3e0c4b074b8984\",");
            lines.Add("\"title\": \"Map 1\",");
            lines.Add("\"rootTopic\": {");
            lines.Add("\"id\": \"b9aa22deba98b3b20c7ac8aca2\",");
            lines.Add("\"class\": \"topic\",");
            lines.Add("\"title\": \""+centerNodeContent+"\",");
            lines.Add("\"structureClass\": \"org.xmind.ui.map.unbalanced\",");
            lines.Add("\"titleUnedited\": true,");
            lines.Add("\"children\": {");
            lines.Add("\"attached\": [");

            foreach (string node in childNodes)
            {
                lines.Add("{");
                lines.Add("\"id\": \""+Guid.NewGuid().ToString()+"\",");
                lines.Add("\"title\": \""+node+"\",");
                lines.Add("\"titleUnedited\": true");

                if(childNodes.IndexOf(node) != childNodes.Count - 1)
                    lines.Add("},");
                else
                    lines.Add("}");
            }

            lines.Add("]");
            lines.Add("},");
            lines.Add("\"extensions\": [");
            lines.Add("{");
            lines.Add("\"content\": [");
            lines.Add("{");
            lines.Add("\"content\": \""+ childNodes.Count.ToString() + "\",");
            lines.Add("\"name\": \"right - number\"");
            lines.Add("}");
            lines.Add("],");
            lines.Add("\"provider\": \"org.xmind.ui.map.unbalanced\"");
            lines.Add("}");
            lines.Add("]");
            lines.Add("},");
            lines.Add("\"topicPositioning\": \"fixed\"");
            lines.Add("}");
            lines.Add("]");

            return lines;
        }

        private void createXmindfile()
        {
            string fileCreation_DirectoryPath = application_Dir + "xmind_creator\\";
            string mapConfig_FilePath = fileCreation_DirectoryPath + "content.json";
            string zip_FilePath = application_Dir + "Created_Xmind_File.zip";
            string output_FilePath = application_Dir + "Created_Xmind_File.xmind";

            if (File.Exists(mapConfig_FilePath))
                File.Delete(mapConfig_FilePath);

            List<string> lines = getXmindLines();
            File.WriteAllLines(mapConfig_FilePath, lines.ToArray());

            if (File.Exists(output_FilePath))
                File.Delete(output_FilePath);

            if (File.Exists(zip_FilePath))
                File.Delete(zip_FilePath);

            ZipFile.CreateFromDirectory(fileCreation_DirectoryPath, zip_FilePath);
            File.Copy(zip_FilePath, output_FilePath);
        }

        #endregion

        #region Creating .cid files

        private List<string> getCidLines()
        {
            int nextId = 1;
            int nextYPos = 77;

            List<string> lines = new List<string>();
            lines.Add("<Diagram Version=\"17\">");
            lines.Add("<Nodes>");

            //Adding center node
            lines.Add("<Node Class=\"std:ShapeNode\" Id=\"0\" Version=\"1\">");
            lines.Add("<Bounds>33, 101, 28, 14</Bounds>");
            lines.Add("<ZIndex>0</ZIndex>");
            lines.Add("<LayerIndex>-1</LayerIndex>");
            lines.Add("<HyperLink />");
            lines.Add("<ToolTip />");
            lines.Add("<Locked>False</Locked>");
            lines.Add("<Visible>True</Visible>");
            lines.Add("<Printable>True</Printable>");
            lines.Add("<Brush Id=\"0\" />");
            lines.Add("<Pen>");
            lines.Add("<Brush Id=\"1\" />");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0.5</Width>");
            lines.Add("</Pen>");
            lines.Add("<Font>");
            lines.Add("<Name>Segoe UI</Name>");
            lines.Add("<Size>12</Size>");
            lines.Add("<Unit>3</Unit>");
            lines.Add("<Bold>False</Bold>");
            lines.Add("<Italic>False</Italic>");
            lines.Add("<Underline>False</Underline>");
            lines.Add("<Strikeout>False</Strikeout>");
            lines.Add("<GdiCharSet>1</GdiCharSet>");
            lines.Add("</Font>");
            lines.Add("<ShadowOffsetX>1</ShadowOffsetX>");
            lines.Add("<ShadowOffsetY>1</ShadowOffsetY>");
            lines.Add("<SubordinateGroup>");
            lines.Add("<MainItem Id=\"0\" ClassId=\"std:ShapeNode\" Version=\"1\" />");
            lines.Add("<Attachments>");
            
            foreach (string node in childNodes)
            {
                lines.Add("<Attachment>");
                lines.Add("<Item Id=\""+ nextId.ToString()+"\" ClassId=\"std:ShapeNode\" Version=\"1\" />");
                lines.Add("<Type>0</Type>");
                lines.Add("<Data>0</Data>");
                lines.Add("<Percents>0, 0, 0, 0</Percents>");
                lines.Add("</Attachment>");
                nextId++;
            }
            nextId = 1;

            lines.Add("</Attachments>");
            lines.Add("<Visible>True</Visible>");
            lines.Add("<AutoDeleteItems>False</AutoDeleteItems>");
            lines.Add("<Expandable>False</Expandable>");
            lines.Add("<FollowMasterRotation>0</FollowMasterRotation>");
            lines.Add("<PrevRect>33, 101, 28, 14</PrevRect>");
            lines.Add("<FollowMasterContainment>False</FollowMasterContainment>");
            lines.Add("</SubordinateGroup>");
            lines.Add("<Weight>1</Weight>");
            lines.Add("<IgnoreLayout>False</IgnoreLayout>");
            lines.Add("<Id Type=\"1\">0</Id>");
            lines.Add("<TextPadding>0, 0, 0, 0</TextPadding>");
            lines.Add("<TextBrush Id=\"1\" />");
            lines.Add("<Obstacle>True</Obstacle>");
            lines.Add("<AllowIncomingLinks>True</AllowIncomingLinks>");
            lines.Add("<AllowOutgoingLinks>True</AllowOutgoingLinks>");
            lines.Add("<EnabledHandles>1023</EnabledHandles>");
            lines.Add("<Expanded>True</Expanded>");
            lines.Add("<Expandable>True</Expandable>");
            lines.Add("<RotationAngle>0</RotationAngle>");
            lines.Add("<Effects />");
            lines.Add("<HandlesStyle>8</HandlesStyle>");
            lines.Add("<Shape Id=\"Rectangle\" />");
            lines.Add("<ShapeOrientation>0</ShapeOrientation>");
            lines.Add("<Text>"+centerNodeContent+"</Text>");
            lines.Add("<TextFormat>");
            lines.Add("<Alignment>1</Alignment>");
            lines.Add("<Flags>16384</Flags>");
            lines.Add("<LineAlignment>1</LineAlignment>");
            lines.Add("<Trimming>1</Trimming>");
            lines.Add("</TextFormat>");
            lines.Add("<PolygonalTextLayout>True</PolygonalTextLayout>");
            lines.Add("<EnableStyledText>False</EnableStyledText>");
            lines.Add("<ImageUrl />");
            lines.Add("<ImageAlign>1</ImageAlign>");
            lines.Add("<RotateImage>True</RotateImage>");
            lines.Add("<RotateText>True</RotateText>");
            lines.Add("<Transparent>False</Transparent>");
            lines.Add("<CustomDraw>0</CustomDraw>");
            lines.Add("<AllowFlip>False</AllowFlip>");
            lines.Add("<FlipX>False</FlipX>");
            lines.Add("<FlipY>False</FlipY>");
            lines.Add("</Node>");

            //Adding child nodes
            foreach (string node in childNodes)
            {
                lines.Add("<Node Class=\"std:ShapeNode\" Id=\""+ nextId.ToString()+"\" Version=\"1\">");
                lines.Add("<Bounds>81, "+ nextYPos.ToString() + ", 28, 14</Bounds>");
                lines.Add("<ZIndex>2</ZIndex>");
                lines.Add("<LayerIndex>-1</LayerIndex>");
                lines.Add("<HyperLink />");
                lines.Add("<ToolTip />");
                lines.Add("<Locked>False</Locked>");
                lines.Add("<Visible>True</Visible>");
                lines.Add("<Printable>True</Printable>");
                lines.Add("<Brush Id=\"0\"/>");
                lines.Add("<Pen>");
                lines.Add("<Color>#FF000000</Color>");
                lines.Add("<DashOffset>0</DashOffset>");
                lines.Add("<DashStyle>0</DashStyle>");
                lines.Add("<LineJoint>0</LineJoint>");
                lines.Add("<MiterLimit>10</MiterLimit>");
                lines.Add("<Width>0.5</Width>");
                lines.Add("</Pen>");
                lines.Add("<Font>");
                lines.Add("<Name>Microsoft Sans Serif</Name>");
                lines.Add("<Size>8.25</Size>");
                lines.Add("<Unit>3</Unit>");
                lines.Add("<Bold>False</Bold>");
                lines.Add("<Italic>False</Italic>");
                lines.Add("<Underline>False</Underline>");
                lines.Add("<Strikeout>False</Strikeout>");
                lines.Add("<GdiCharSet>1</GdiCharSet>");
                lines.Add("</Font>");
                lines.Add("<ShadowOffsetX>1</ShadowOffsetX>");
                lines.Add("<ShadowOffsetY>1</ShadowOffsetY>");
                lines.Add("<Weight>1</Weight>");
                lines.Add("<IgnoreLayout>False</IgnoreLayout>");
                lines.Add("<Id Type=\"1\">"+ nextId.ToString()+"</Id>");
                lines.Add("<TextPadding>0, 0, 0, 0</TextPadding>");
                lines.Add("<TextBrush Id=\"1\" />");
                lines.Add("<Obstacle>True</Obstacle>");
                lines.Add("<AllowIncomingLinks>True</AllowIncomingLinks>");
                lines.Add("<AllowOutgoingLinks>True</AllowOutgoingLinks>");
                lines.Add("<EnabledHandles>1023</EnabledHandles>");
                lines.Add("<Expanded>True</Expanded>");
                lines.Add("<Expandable>False</Expandable>");
                lines.Add("<RotationAngle>0</RotationAngle>");
                lines.Add("<Effects />");
                lines.Add("<HandlesStyle>8</HandlesStyle>");
                lines.Add("<Shape Id=\"Rectangle\" />");
                lines.Add("<ShapeOrientation>0</ShapeOrientation>");
                lines.Add("<Text>"+node+"</Text>");
                lines.Add("<TextFormat>");
                lines.Add("<Alignment>1</Alignment>");
                lines.Add("<Flags>1024</Flags>");
                lines.Add("<LineAlignment>1</LineAlignment>");
                lines.Add("<Trimming>1</Trimming>");
                lines.Add("</TextFormat>");
                lines.Add("<PolygonalTextLayout>False</PolygonalTextLayout>");
                lines.Add("<EnableStyledText>False</EnableStyledText>");
                lines.Add("<ImageUrl />");
                lines.Add("<ImageAlign>1</ImageAlign>");
                lines.Add("<RotateImage>True</RotateImage>");
                lines.Add("<RotateText>False</RotateText>");
                lines.Add("<Transparent>False</Transparent>");
                lines.Add("<CustomDraw>0</CustomDraw>");
                lines.Add("<AllowFlip>False</AllowFlip>");
                lines.Add("<FlipX>False</FlipX>");
                lines.Add("<FlipY>False</FlipY>");
                lines.Add("</Node>");

                nextId++;
                nextYPos = nextYPos + 24;
            }

            lines.Add("</Nodes>");
            lines.Add("<Links>");

            int linkTarget = 1;
            nextYPos = 77;

            //Adding links between nodes
            foreach (string node in childNodes)
            {
                lines.Add("<Link Class=\"std:DiagramLink\" Id=\""+nextId.ToString()+"\" Version=\"2\">");
                lines.Add("<Bounds>46.30208, 77.78749, 53.975, 31.75</Bounds>");
                lines.Add("<ZIndex>5</ZIndex>");
                lines.Add("<LayerIndex>-1</LayerIndex>");
                lines.Add("<HyperLink />");
                lines.Add("<ToolTip />");
                lines.Add("<Locked>False</Locked>");
                lines.Add("<Visible>True</Visible>");
                lines.Add("<Printable>True</Printable>");
                lines.Add("<Pen>");
                lines.Add("<Color>#FF000000</Color>");
                lines.Add("<DashOffset>0</DashOffset>");
                lines.Add("<DashStyle>0</DashStyle>");
                lines.Add("<LineJoint>0</LineJoint>");
                lines.Add("<MiterLimit>10</MiterLimit>");
                lines.Add("<Width>0.5</Width>");
                lines.Add("</Pen>");
                lines.Add("<ShadowOffsetX>1</ShadowOffsetX>");
                lines.Add("<ShadowOffsetY>1</ShadowOffsetY>");
                lines.Add("<Weight>1</Weight>");
                lines.Add("<IgnoreLayout>False</IgnoreLayout>");
                lines.Add("<Id Type=\"1\">"+nextId.ToString()+"</Id>");
                lines.Add("<TextPadding>0, 0, 0, 0</TextPadding>");
                lines.Add("<Origin Id=\"0\" ClassId=\"std:ShapeNode\" Version=\"1\" />");
                lines.Add("<OriginRelative>100, 50</OriginRelative>");
                lines.Add("<OriginAnchor>-1</OriginAnchor>");
                lines.Add("<Destination Id=\"" + linkTarget.ToString() + "\" ClassId=\"std:ShapeNode\" Version=\"1\" />"); ;
                lines.Add("<DestinationRelative>0, 50</DestinationRelative>");
                lines.Add("<DestinationAnchor>-1</DestinationAnchor>");
                lines.Add("<Shape>1</Shape>");
                lines.Add("<SegmentCount>1</SegmentCount>");
                lines.Add("<Points>");
                lines.Add("<Point>61, 108</Point>");
                lines.Add("<Point>81, "+ (nextYPos + 7).ToString() + "</Point>");
                lines.Add("</Points>");
                lines.Add("<Dynamic>True</Dynamic>");
                lines.Add("<RetainForm>True</RetainForm>");
                lines.Add("<AutoRoute>False</AutoRoute>");
                lines.Add("<AutoSnapToNode>False</AutoSnapToNode>");
                lines.Add("<CascadeOrientation>0</CascadeOrientation>");
                lines.Add("<CascadeStartHorizontal>True</CascadeStartHorizontal>");
                lines.Add("<Text />");
                lines.Add("<TextStyle>2</TextStyle>");
                lines.Add("<HeadPen>");
                lines.Add("<Color>#FF000000</Color>");
                lines.Add("<DashOffset>0</DashOffset>");
                lines.Add("<DashStyle>0</DashStyle>");
                lines.Add("<LineJoint>0</LineJoint>");
                lines.Add("<MiterLimit>10</MiterLimit>");
                lines.Add("<Width>0.5</Width>");
                lines.Add("</HeadPen>");
                lines.Add("<HeadShape Id=\"Triangle\" />");
                lines.Add("<HeadShapeSize>5</HeadShapeSize>");
                lines.Add("<BaseShapeSize>5</BaseShapeSize>");
                lines.Add("<IntermediateShapeSize>5</IntermediateShapeSize>");
                lines.Add("<CustomDraw>0</CustomDraw>");
                lines.Add("<DrawCrossings>True</DrawCrossings>");
                lines.Add("<HandlesStyle>1</HandlesStyle>");
                lines.Add("<AllowMoveStart>True</AllowMoveStart>");
                lines.Add("<AllowMoveEnd>True</AllowMoveEnd>");
                lines.Add("<HeadBrush Id=\"1\" />");
                lines.Add("</Link>");

                nextId++;
                linkTarget++;
                nextYPos = nextYPos + 24;
            }

            lines.Add("</Links>");
            lines.Add("<Properties>");
            lines.Add("<ItemCounter>" + ((childNodes.Count * 2) + 1).ToString() + "</ItemCounter>");
            lines.Add("<LinkHeadShape Id=\"Triangle\"/>");
            lines.Add("<LinkHeadShapeSize>5</LinkHeadShapeSize>");
            lines.Add("<LinkBaseShapeSize>5</LinkBaseShapeSize>");
            lines.Add("<LinkIntermediateShapeSize>5</LinkIntermediateShapeSize>");
            lines.Add("<AlignToGrid>True</AlignToGrid>");
            lines.Add("<ShowGrid>False</ShowGrid>");
            lines.Add("<GridStyle>0</GridStyle>");
            lines.Add("<GridColor>#FF8C8C96</GridColor>");
            lines.Add("<GridSizeX>2</GridSizeX>");
            lines.Add("<GridSizeY>2</GridSizeY>");
            lines.Add("<GridOffsetX>NaN</GridOffsetX>");
            lines.Add("<GridOffsetY>NaN</GridOffsetY>");
            lines.Add("<ShadowBrush Id=\"2\"/>");
            lines.Add("<ShadowsStyle>0</ShadowsStyle>");
            lines.Add("<ShadowOffsetX>1</ShadowOffsetX>");
            lines.Add("<ShadowOffsetY>1</ShadowOffsetY>");
            lines.Add("<ImageAlign>2</ImageAlign>");
            lines.Add("<TextColor>#FF000000</TextColor>");
            lines.Add("<LinkStyle>1</LinkStyle>");
            lines.Add("<LinkSegments>1</LinkSegments>");
            lines.Add("<Bounds>0,0,184.6792,230.9812</Bounds>");
            lines.Add("<TableRowsCount>4</TableRowsCount>");
            lines.Add("<TableColumnsCount>2</TableColumnsCount>");
            lines.Add("<TableColumnWidth>18</TableColumnWidth>");
            lines.Add("<TableRowHeight>5</TableRowHeight>");
            lines.Add("<TableCaptionHeight>5</TableCaptionHeight>");
            lines.Add("<TableCaption>Table</TableCaption>");
            lines.Add("<LinkCascadeOrientation>0</LinkCascadeOrientation>");
            lines.Add("<TableCellBorders>2</TableCellBorders>");
            lines.Add("<AutoHighlightRows>False</AutoHighlightRows>");
            lines.Add("<NodesExpandable>False</NodesExpandable>");
            lines.Add("<TablesScrollable>False</TablesScrollable>");
            lines.Add("<ToolTip/>");
            lines.Add("<DefaultTextFormat>");
            lines.Add("<Alignment>1</Alignment>");
            lines.Add("<Flags>1024</Flags>");
            lines.Add("<LineAlignment>1</LineAlignment>");
            lines.Add("<Trimming>1</Trimming>");
            lines.Add("</DefaultTextFormat>");
            lines.Add("<Font>");
            lines.Add("<Name>MicrosoftSansSerif</Name>");
            lines.Add("<Size>9.75</Size>");
            lines.Add("<Unit>3</Unit>");
            lines.Add("<Bold>False</Bold>");
            lines.Add("<Italic>False</Italic>");
            lines.Add("<Underline>False</Underline>");
            lines.Add("<Strikeout>False</Strikeout>");
            lines.Add("<GdiCharSet>1</GdiCharSet>");
            lines.Add("</Font>");
            lines.Add("<ImageDpiX>0</ImageDpiX>");
            lines.Add("<ImageDpiY>0</ImageDpiY>");
            lines.Add("<DefaultShape Id=\"Rectangle\"/>");
            lines.Add("<ShapePen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</ShapePen>");
            lines.Add("<LinkPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</LinkPen>");
            lines.Add("<TablePen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</TablePen>");
            lines.Add("<ContainerHighlightPen>");
            lines.Add("<Color>#FF87CEFA</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0.5</Width>");
            lines.Add("</ContainerHighlightPen>");
            lines.Add("<ShapeBrush Id=\"3\"/>");
            lines.Add("<LinkBrush Id=\"4\"/>");
            lines.Add("<TableBrush Id=\"5\"/>");
            lines.Add("<Brush Id=\"6\"/>");
            lines.Add("<RowHighlightBrush Id=\"7\"/>");
            lines.Add("<ShapeHandlesStyle>8</ShapeHandlesStyle>");
            lines.Add("<TableHandlesStyle>2</TableHandlesStyle>");
            lines.Add("<AllowLinksRepeat>False</AllowLinksRepeat>");
            lines.Add("<AllowUnconnectedLinks>True</AllowUnconnectedLinks>");
            lines.Add("<AllowUnanchoredLinks>True</AllowUnanchoredLinks>");
            lines.Add("<PolygonalTextLayout>False</PolygonalTextLayout>");
            lines.Add("<ShapeOrientation>0</ShapeOrientation>");
            lines.Add("<RouteLinks>False</RouteLinks>");
            lines.Add("<LinksRetainForm>True</LinksRetainForm>");
            lines.Add("<LinkTextStyle>0</LinkTextStyle>");
            lines.Add("<TableConnectionStyle>1</TableConnectionStyle>");
            lines.Add("<MeasureUnit>6</MeasureUnit>");
            lines.Add("<MinBounds>0,0,184.6792,230.9812</MinBounds>");
            lines.Add("<ExpandBtnPos>0</ExpandBtnPos>");
            lines.Add("<EnableStyledText>False</EnableStyledText>");
            lines.Add("<LinkText/>");
            lines.Add("<ShapeText/>");
            lines.Add("<LinkCrossings>0</LinkCrossings>");
            lines.Add("<CrossRadius>1.5</CrossRadius>");
            lines.Add("<AutoSnapLinks>False</AutoSnapLinks>");
            lines.Add("<AutoSnapDistance>16</AutoSnapDistance>");
            lines.Add("<LinkHandlesStyle>1</LinkHandlesStyle>");
            lines.Add("<RoundedLinks>False</RoundedLinks>");
            lines.Add("<RoundedLinksRadius>2</RoundedLinksRadius>");
            lines.Add("<EnableLanes>False</EnableLanes>");
            lines.Add("<Lane>");
            lines.Add("<LeftMargin>0</LeftMargin>");
            lines.Add("<TopMargin>0</TopMargin>");
            lines.Add("<MinHeaderSize>10</MinHeaderSize>");
            lines.Add("<DefaultFont>");
            lines.Add("<Name>MicrosoftSansSerif</Name>");
            lines.Add("<Size>9</Size>");
            lines.Add("<Unit>3</Unit>");
            lines.Add("<Bold>False</Bold>");
            lines.Add("<Italic>False</Italic>");
            lines.Add("<Underline>False</Underline>");
            lines.Add("<Strikeout>False</Strikeout>");
            lines.Add("<GdiCharSet>1</GdiCharSet>");
            lines.Add("</DefaultFont>");
            lines.Add("<HookHeaders>True</HookHeaders>");
            lines.Add("<HeadersOnTop>True</HeadersOnTop>");
            lines.Add("<AlignCells>True</AlignCells>");
            lines.Add("<RowWidths/>");
            lines.Add("<ColumnHeights/>");
            lines.Add("<AllowResizeHeaders>True</AllowResizeHeaders>");
            lines.Add("<Columns>");
            lines.Add("<Width>0</Width>");
            lines.Add("<Height>0</Height>");
            lines.Add("<ResizeType>1</ResizeType>");
            lines.Add("<Title/>");
            lines.Add("<RotateTitle>False</RotateTitle>");
            lines.Add("<TitleColor>#FF000000</TitleColor>");
            lines.Add("<Style>");
            lines.Add("<BackgroundBrush Id=\"8\"/>");
            lines.Add("<LeftBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</LeftBorderPen>");
            lines.Add("<TopBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</TopBorderPen>");
            lines.Add("<RightBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</RightBorderPen>");
            lines.Add("<BottomBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</BottomBorderPen>");
            lines.Add("</Style>");
            lines.Add("<SubHeaders/>");
            lines.Add("</Columns>");
            lines.Add("<Rows>");
            lines.Add("<Width>0</Width>");
            lines.Add("<Height>0</Height>");
            lines.Add("<ResizeType>1</ResizeType>");
            lines.Add("<Title/>");
            lines.Add("<RotateTitle>False</RotateTitle>");
            lines.Add("<TitleColor>#FF000000</TitleColor>");
            lines.Add("<Style>");
            lines.Add("<BackgroundBrush Id=\"8\"/>");
            lines.Add("<LeftBorderPen>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</LeftBorderPen>");
            lines.Add("<TopBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</TopBorderPen>");
            lines.Add("<RightBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</RightBorderPen>");
            lines.Add("<BottomBorderPen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</BottomBorderPen>");
            lines.Add("</Style>");
            lines.Add("<SubHeaders/>");
            lines.Add("</Rows>");
            lines.Add("<Cells ColumnCount=\"0\" RowCount=\"0\"/>");
            lines.Add("</Lane>");
            lines.Add("<ContainerMargin>10</ContainerMargin>");
            lines.Add("<ContainerMinimumSize>50,40</ContainerMinimumSize>");
            lines.Add("<ContainerCaption>Container</ContainerCaption>");
            lines.Add("<ContainerCaptionHeight>5</ContainerCaptionHeight>");
            lines.Add("<ContainersFoldable>True</ContainersFoldable>");
            lines.Add("<ShapeCustomDraw>0</ShapeCustomDraw>");
            lines.Add("<SelectAfterCreate>False</SelectAfterCreate>");
            lines.Add("<AutoResize>2</AutoResize>");
            lines.Add("<RestrictItemsToBounds>0</RestrictItemsToBounds>");
            lines.Add("<SnapToAnchor>1</SnapToAnchor>");
            lines.Add("<AllowSplitLinks>True</AllowSplitLinks>");
            lines.Add("<AllowSelfLoops>False</AllowSelfLoops>");
            lines.Add("<ExpandOnIncoming>False</ExpandOnIncoming>");
            lines.Add("<RecursiveExpand>False</RecursiveExpand>");
            lines.Add("<MergeThreshold>0</MergeThreshold>");
            lines.Add("<ShowAnchors>2</ShowAnchors>");
            lines.Add("<SelectionOnTop>True</SelectionOnTop>");
            lines.Add("<AdjustmentHandlesSize>2</AdjustmentHandlesSize>");
            lines.Add("<ShowHandlesOnDrag>True</ShowHandlesOnDrag>");
            lines.Add("<ShowDisabledHandles>True</ShowDisabledHandles>");
            lines.Add("<TableStyle>0</TableStyle>");
            lines.Add("<LinkEndsMovable>True</LinkEndsMovable>");
            lines.Add("<DynamicLinks>True</DynamicLinks>");
            lines.Add("<HitTestPriority>1</HitTestPriority>");
            lines.Add("<AllowMultipleSelection>True</AllowMultipleSelection>");
            lines.Add("<LinkBranchIndicator>0</LinkBranchIndicator>");
            lines.Add("<BranchIndicatorSize>4</BranchIndicatorSize>");
            lines.Add("<BranchIndicatorColor>#FF90EE90</BranchIndicatorColor>");
            lines.Add("<LinkMergeIndicator>0</LinkMergeIndicator>");
            lines.Add("<MergeIndicatorSize>4</MergeIndicatorSize>");
            lines.Add("<MergeIndicatorColor>#FF90EE90</MergeIndicatorColor>");
            lines.Add("<ActiveItemHandlesStyle>");
            lines.Add("<HandlePen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</HandlePen>");
            lines.Add("<HandleBrush Id=\"8\"/>");
            lines.Add("<DashPen>");
            lines.Add("<Brush Id=\"9\"/>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</DashPen>");
            lines.Add("<HatchBrush Id=\"10\"/>");
            lines.Add("<PatternBrush Id=\"11\"/>");
            lines.Add("<ControlPointBrush Id=\"12\"/>");
            lines.Add("</ActiveItemHandlesStyle>");
            lines.Add("<SelectedItemHandlesStyle>");
            lines.Add("<HandlePen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</HandlePen>");
            lines.Add("<HandleBrush Id=\"13\"/>");
            lines.Add("<DashPen>");
            lines.Add("<Brush Id=\"14\"/>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</DashPen>");
            lines.Add("<HatchBrush Id=\"10\"/>");
            lines.Add("<PatternBrush Id=\"11\"/>");
            lines.Add("<ControlPointBrush Id=\"12\"/>");
            lines.Add("</SelectedItemHandlesStyle>");
            lines.Add("<DisabledHandlesStyle>");
            lines.Add("<HandlePen>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</HandlePen>");
            lines.Add("<HandleBrush Id=\"15\"/>");
            lines.Add("<DashPen>");
            lines.Add("<Brush Id=\"9\"/>");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("<DashOffset>0</DashOffset>");
            lines.Add("<DashStyle>0</DashStyle>");
            lines.Add("<LineJoint>0</LineJoint>");
            lines.Add("<MiterLimit>10</MiterLimit>");
            lines.Add("<Width>0</Width>");
            lines.Add("</DashPen>");
            lines.Add("<HatchBrush Id=\"10\"/>");
            lines.Add("<PatternBrush Id=\"11\"/>");
            lines.Add("<ControlPointBrush Id=\"12\"/>");
            lines.Add("</DisabledHandlesStyle>");
            lines.Add("</Properties>");
            lines.Add("<Effects />");
            lines.Add("<Selection />");
            lines.Add("<Style Id=\"0\" />");
            lines.Add("<Theme>");
            lines.Add("<Style TargetType=\"std:DiagramLink\" Class=\"std:DiagramLinkStyle\" Version=\"1\">");
            lines.Add("<Property Name=\"Brush\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"1\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"Stroke\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"1\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"StrokeThickness\">");
            lines.Add("<Value Type=\"6\">0.5</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"HeadStrokeThickness\">");
            lines.Add("<Value Type=\"6\">0.5</Value>");
            lines.Add("</Property>");
            lines.Add("</Style>");
            lines.Add("<Style TargetType=\"std:Diagram\" Class=\"std:DiagramStyle\" Version=\"1\">");
            lines.Add("<Property Name=\"FontFamily\">");
            lines.Add("<Value Type=\"1\">Segoe UI</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"FontSize\">");
            lines.Add("<Value Type=\"11\">12</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"FontStyle\">");
            lines.Add("<Value Type=\"a:FontStyle\">0</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"FontUnit\">");
            lines.Add("<Value Type=\"a:FontUnit\">3</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"Brush\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"0\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"TextBrush\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"1\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("</Style>");
            lines.Add("<Style TargetType=\"std:ShapeNode\" Class=\"std:ShapeNodeStyle\" Version=\"1\">");
            lines.Add("<Property Name=\"Brush\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"0\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"Stroke\">");
            lines.Add("<Value Type=\"100\">");
            lines.Add("<Brush Id=\"1\" />");
            lines.Add("</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"StrokeThickness\">");
            lines.Add("<Value Type=\"6\">0.5</Value>");
            lines.Add("</Property>");
            lines.Add("</Style>");
            lines.Add("</Theme>");
            lines.Add("<Resources>");
            lines.Add("<Styles>");
            lines.Add("<Style Index=\"0\" Class=\"std:DiagramStyle\" Version=\"1\">");
            lines.Add("<Property Name=\"FontFamily\">");
            lines.Add("<Value Type=\"1\">Segoe UI</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"FontSize\">");
            lines.Add("<Value Type=\"11\">12</Value>");
            lines.Add("</Property>");
            lines.Add("<Property Name=\"FontStyle\">");
            lines.Add("<Value Type=\"a:FontStyle\">0</Value>");
            lines.Add("</Property>");
            lines.Add("</Style>");
            lines.Add("</Styles>");
            lines.Add("<Effects />");
            lines.Add("<Brushes>");
            lines.Add("<Brush Index=\"2\" Type=\"Solid\">");
            lines.Add("<Color>#96A9A9A9</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"7\" Type=\"Solid\">");
            lines.Add("<Color>#FFB9D1EA</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"8\" Type=\"Solid\">");
            lines.Add("<Color>#FFFFFFFF</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"1\" Type=\"Solid\">");
            lines.Add("<Color>#FF000000</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"13\" Type=\"Solid\">");
            lines.Add("<Color>#FFAAAAAA</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"9\" Type=\"Pattern\">");
            lines.Add("<ForeColor>#FFFFFFFF</ForeColor>");
            lines.Add("<BackColor>#FF000000</BackColor>");
            lines.Add("<Style>20</Style>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"6\" Type=\"Solid\">");
            lines.Add("<Color>#FFB0E0E6</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"0\" Type=\"Solid\">");
            lines.Add(" <Color>#FFFFFFC0</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"5\" Type=\"Solid\">");
            lines.Add("<Color>#FFB4A0A0</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"14\" Type=\"Pattern\">");
            lines.Add("<ForeColor>#FFD3D3D3</ForeColor>");
            lines.Add("<BackColor>#FF000000</BackColor>");
            lines.Add("<Style>20</Style>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"12\" Type=\"Solid\">");
            lines.Add("<Color>#FFFFFF00</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"15\" Type=\"Solid\">");
            lines.Add("<Color>#FFC80000</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"11\" Type=\"Pattern\">");
            lines.Add("<ForeColor>#FF000000</ForeColor>");
            lines.Add("<BackColor>#32000000</BackColor>");
            lines.Add("<Style>12</Style>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"10\" Type=\"Pattern\">");
            lines.Add("<ForeColor>#FF000000</ForeColor>");
            lines.Add("<BackColor>#FFFFFFFF</BackColor>");
            lines.Add("<Style>3</Style>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"4\" Type=\"Solid\">");
            lines.Add("<Color>#FF78DCFF</Color>");
            lines.Add("</Brush>");
            lines.Add("<Brush Index=\"3\" Type=\"Solid\">");
            lines.Add("<Color>#FFDCDCFF</Color>");
            lines.Add("</Brush>");
            lines.Add("</Brushes>");
            lines.Add("<Images Count=\"0\" />");
            lines.Add("<Svgs Count=\"0\" />");
            lines.Add("<Stencils Count=\"0\" />");
            lines.Add("<Maps Count=\"0\" />");
            lines.Add("</Resources>");
            lines.Add("<Layers ActiveLayer=\"-1\"/>");
            lines.Add("<View>");
            lines.Add("<Behavior>3</Behavior>");
            lines.Add("<ScrollX>0</ScrollX>");
            lines.Add("<ScrollY>0</ScrollY>");
            lines.Add("<ZoomFactor>100</ZoomFactor>");
            lines.Add("<AllowInplaceEdit>True</AllowInplaceEdit>");
            lines.Add("<InplaceEditAcceptOnEnter>False</InplaceEditAcceptOnEnter>");
            lines.Add("<InplaceEditCancelOnEsc>True</InplaceEditCancelOnEsc>");
            lines.Add("<InplaceEditFont>");
            lines.Add("<Name>Microsoft Sans Serif</Name>");
            lines.Add("<Size>8.25</Size>");
            lines.Add("<Unit>3</Unit>");
            lines.Add("<Bold>False</Bold>");
            lines.Add("<Italic>False</Italic>");
            lines.Add("<Underline>False</Underline>");
            lines.Add("<Strikeout>False</Strikeout>");
            lines.Add("<GdiCharSet>0</GdiCharSet>");
            lines.Add("</InplaceEditFont>");
            lines.Add("<ModificationStart>0</ModificationStart>");
            lines.Add("<AutoScroll>True</AutoScroll>");
            lines.Add("</View>");
            lines.Add("</Diagram>");

            return lines;
        }

        private void createCidfile()
        {
            string fileCreation_DirectoryPath = application_Dir + "cid_creator\\";
            string mapConfig_FilePath = fileCreation_DirectoryPath + "map.map";
            string zip_FilePath = application_Dir + "Created_Cid_File.zip";
            string output_FilePath = application_Dir + "Created_Cid_File.cid";

            if (File.Exists(mapConfig_FilePath))
                File.Delete(mapConfig_FilePath);

            List<string> lines = getCidLines();
            File.WriteAllLines(mapConfig_FilePath, lines.ToArray());

            if (File.Exists(output_FilePath))
                File.Delete(output_FilePath);

            if (File.Exists(zip_FilePath))
                File.Delete(zip_FilePath);

            ZipFile.CreateFromDirectory(fileCreation_DirectoryPath, zip_FilePath);
            File.Copy(zip_FilePath, output_FilePath);
        }


        #endregion

        #region Opening files with applications

        //Open a file with an executable by using a cmd prompt to call the executable
        private void OpenWithCmd(string FilePath, string ExecutablePath)
        {
            string cmd = "\"\"" + ExecutablePath + "\" \"" + FilePath + "\"\"";

            ProcessStartInfo cmdProcess = new ProcessStartInfo("cmd.exe", "/K " + cmd);
            cmdProcess.CreateNoWindow = true;
            cmdProcess.UseShellExecute = false;

            Process.Start(cmdProcess);
        }

        //Open a file with an executable by calling the executable directly
        private void OpenWith(string FilePath, string ExecutablePath)
        {
            ProcessStartInfo openProcess = new ProcessStartInfo(FilePath)
            {
                Arguments = Path.GetFileName(FilePath),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(FilePath),
                FileName = ExecutablePath,
                Verb = "OPEN"
            };

            Process.Start(openProcess);
        }

        #endregion

        #region Buttons for adding/removing nodes

        private void addChildModeButton_Click(object sender, RoutedEventArgs e)
        {
            childNodes.Add(childNodeContent);
        }

        private void deleteNodeButton_Click(object sender, RoutedEventArgs e)
        {
            if(childNodeList.SelectedItem != null)
                childNodes.Remove(childNodeList.SelectedItem.ToString());
        }

        #endregion
    }
}
