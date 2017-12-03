
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ScoreCalculator
{
    class VMScores : INotifyPropertyChanged
    {
        private decimal[][] studentScore;
        private const int NBR_SECT = 3;
        private const int ZERO = 0;
        private string FOLDER_NAME = "datasource";
        private string FILE_NAME = "Section";
        private List<List<decimal>> examScore;
        private List<decimal> maxScore, minScore, avgScore;
        private decimal avgAllSect, highScore, lowScore;
        private string highSectNbr, lowSectNbr;

        //constructor
        #region Constructor
        public VMScores()
        {
            studentScore = new decimal[NBR_SECT][];
            maxScore = new List<decimal>();
            minScore = new List<decimal>();
            avgScore = new List<decimal>();

            examScore = new List<List<decimal>>();
            for (int i = 0; i < NBR_SECT; i++)
            {
                examScore.Add(new List<decimal>());
                maxScore.Add(ZERO);
                minScore.Add(ZERO);
                avgScore.Add(ZERO);
            }
        }
        #endregion

        //Enum: Sections
        #region Enum Sections
        private enum Sections : uint
        {
            S1 = 0,
            S2,
            S3
        }
        #endregion

        //public properties: Lists
        #region Lists
        public List<decimal> ExamScoreS1
        {
            get { return examScore[(int)Sections.S1]; }
            set { examScore[(int)Sections.S1] = value; OnPropertyChanged(); }
        }
        public List<decimal> ExamScoreS2
        {
            get { return examScore[(int)Sections.S2]; }
            set { examScore[(int)Sections.S2] = value; OnPropertyChanged(); }
        }
        public List<decimal> ExamScoreS3
        {
            get { return examScore[(int)Sections.S3]; }
            set { examScore[(int)Sections.S3] = value; OnPropertyChanged(); }
        }
        #endregion

        //public properties: Max values
        #region Maximum
        public decimal MaxScoreS1
        {
            get { return maxScore[(int)Sections.S1]; }
            set { maxScore[(int)Sections.S1] = value; OnPropertyChanged(); }
        }
        public decimal MaxScoreS2
        {
            get { return maxScore[(int)Sections.S2]; }
            set { maxScore[(int)Sections.S2] = value; OnPropertyChanged(); }
        }
        public decimal MaxScoreS3
        {
            get { return maxScore[(int)Sections.S3]; }
            set { maxScore[(int)Sections.S3] = value; OnPropertyChanged(); }
        }
        #endregion

        //public properties: Min values
        #region Minimum
        public decimal MinScoreS1
        {
            get { return minScore[(int)Sections.S1]; }
            set { minScore[(int)Sections.S1] = value; OnPropertyChanged(); }
        }
        public decimal MinScoreS2
        {
            get { return minScore[(int)Sections.S2]; }
            set { minScore[(int)Sections.S2] = value; OnPropertyChanged(); }
        }
        public decimal MinScoreS3
        {
            get { return minScore[(int)Sections.S3]; }
            set { minScore[(int)Sections.S3] = value; OnPropertyChanged(); }
        }
        #endregion

        //public properties: Averages
        #region Averages
        public decimal AvgScoreS1
        {
            get { return avgScore[(int)Sections.S1]; }
            set { avgScore[(int)Sections.S1] = value; OnPropertyChanged(); }
        }
        public decimal AvgScoreS2
        {
            get { return avgScore[(int)Sections.S2]; }
            set { avgScore[(int)Sections.S2] = value; OnPropertyChanged(); }
        }
        public decimal AvgScoreS3
        {
            get { return avgScore[(int)Sections.S3]; }
            set { avgScore[(int)Sections.S3] = value; OnPropertyChanged(); }
        }
        #endregion

        //public properties: Highest & Lowest
        #region Global score: Average, Highest, Lowest, Section
        public decimal AvgAllSect
        {
            get { return avgAllSect; }
            set { avgAllSect = value; OnPropertyChanged(); }
        }

        public decimal HighScore
        {
            get { return highScore; }
            set { highScore = value; OnPropertyChanged(); }
        }
        public string HighSectNbr
        {
            get { return highSectNbr; }
            set { highSectNbr = value; OnPropertyChanged(); }
        }

        public decimal LowScore
        {
            get { return lowScore; }
            set { lowScore = value; OnPropertyChanged(); }
        }
        public string LowSectNbr
        {
            get { return lowSectNbr; }
            set { lowSectNbr = value; OnPropertyChanged(); }
        }
        #endregion

        #region GetScores from files and Jagged Array
        public bool GetScores(ref Exception ex)
        {
            int iFile, iScore;
            string folderLoc;
            string[] sectFiles, fileContent;
            //List<string> lstSectFiles = new List<string>();

            try
            {
                //Create instance of objects
                ExamScoreS1 = new List<decimal>();
                ExamScoreS2 = new List<decimal>();
                ExamScoreS3 = new List<decimal>();

                //Assign 0
                for (int i = 0; i < NBR_SECT; i++)
                    examScore[i].Add(ZERO);

                //Read Files 
                folderLoc = FOLDER_NAME;    // Path.Combine(System.Environment.CurrentDirectory, FOLDER_NAME);
                sectFiles = Directory.GetFiles(folderLoc, (FILE_NAME + "*"), SearchOption.TopDirectoryOnly);
                //lstSectFiles = sectFiles.ToList<string>();

                iFile = 0;
                foreach (string sFile in sectFiles)
                {
                    iScore = 0;
                    fileContent = File.ReadAllLines(sFile);

                    //create an instance for the sub-array iFile
                    studentScore[iFile] = new decimal[fileContent.Length];

                    foreach (string stScore in fileContent)
                    {
                        studentScore[iFile][iScore++] = decimal.Parse(stScore);
                    }

                    if (studentScore[iFile].Length > 0)
                    {
                        examScore[iFile].Clear();
                        examScore[iFile].AddRange(studentScore[iFile++]);
                    }
                }

                //Show List
                ExamScoreS1 = examScore[(int)Sections.S1];
                ExamScoreS2 = examScore[(int)Sections.S2];
                ExamScoreS3 = examScore[(int)Sections.S3];

                return true;
            }
            catch (Exception e)
            {
                ex = e;
                return false;
            }
        }
        #endregion

        #region Calculate Score
        public void CalculateScore()
        {
            //Average scores for each individual section
            for (int i = 0; i < NBR_SECT; i++)
            {
                maxScore[i] = examScore[i].Max();
                minScore[i] = examScore[i].Min();
                avgScore[i] = examScore[i].Average();
            }

            //Min, Max, Average exam score for all students in all sections
            highScore = maxScore.Max();
            lowScore = minScore.Min();
            avgAllSect = avgScore.Average();

            //Find the section number(s)
            FindSection();

            //Show results
            MinScoreS1 = minScore[(int)Sections.S1];
            MaxScoreS1 = maxScore[(int)Sections.S1];
            AvgScoreS1 = avgScore[(int)Sections.S1];

            MinScoreS2 = minScore[(int)Sections.S2];
            MaxScoreS2 = maxScore[(int)Sections.S2];
            AvgScoreS2 = avgScore[(int)Sections.S2];

            MinScoreS3 = minScore[(int)Sections.S3];
            MaxScoreS3 = maxScore[(int)Sections.S3];
            AvgScoreS3 = avgScore[(int)Sections.S3];

            HighScore = highScore;
            HighSectNbr = highSectNbr;
            LowScore = lowScore;
            LowSectNbr = lowSectNbr;
            AvgAllSect = avgAllSect;
        }
        #endregion

        //Find section number(s) with the highest and lowest score
        #region FindSection 
        private void FindSection()
        {
            highSectNbr = "";
            lowSectNbr = "";

            for (int i = 0; i < NBR_SECT; i++)
            {
                //section number(s) with the highest
                if (maxScore[i] == highScore)
                    if (string.IsNullOrWhiteSpace(highSectNbr))
                        highSectNbr += (i + 1).ToString();
                    else
                        highSectNbr += ", " + (i + 1).ToString();

                //section number(s) with the lowest
                if (minScore[i] == lowScore)
                    if (string.IsNullOrWhiteSpace(lowSectNbr))
                        lowSectNbr += (i + 1).ToString();
                    else
                        lowSectNbr += ", " + (i + 1).ToString();
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            // make sure only to call this if the value actually changes
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        #endregion
    }
}
