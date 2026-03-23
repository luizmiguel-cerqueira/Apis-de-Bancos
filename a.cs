using System;
using System.Security.Cryptography;

// initialize variables - graded assignments 
int currentAssignments = 5;

List<string> students = ["sophia","andrew","emma","logan"];
List<int> score = [90,86,87,98,100,92,89,81,96,90,90,85,87,98,68,90,95,87,99,96];
List<int> sums = new List<int>(students.Count);
string currentStudentLetterGrade = "";


for(int i = 0; i < students.Count;i++)
{
    sums.Add(0);
    for(int j = 0; j < 5 ; j++)
    {
        sums[i]+=score[(i*5)+j];
    }
    
    if (sums[i]/5 >= 97)
        currentStudentLetterGrade = "A+";

    else if (sums[i]/5 >= 93)
        currentStudentLetterGrade = "A";

    else if (sums[i]/5 >= 90)
        currentStudentLetterGrade = "A-";

    else if (sums[i]/5 >= 87)
        currentStudentLetterGrade = "B+";

    else if (sums[i]/5 >= 83)
        currentStudentLetterGrade = "B";

    else if (sums[i]/5 >= 80)
        currentStudentLetterGrade = "B-";

    else if (sums[i]/5 >= 77)
        currentStudentLetterGrade = "C+";

    else if (sums[i]/5 >= 73)
        currentStudentLetterGrade = "C";

    else if (sums[i]/5 >= 70)
        currentStudentLetterGrade = "C-";

    else if (sums[i]/5 >= 67)
        currentStudentLetterGrade = "D+";

    else if (sums[i]/5 >= 63)
        currentStudentLetterGrade = "D";

    else if (sums[i]/5 >= 60)
        currentStudentLetterGrade = "D-";

    else
        currentStudentLetterGrade = "F";
    Console.WriteLine($"{students[i]}\t\t{sums[i]/5}\t{currentStudentLetterGrade}");
}

int sophia1 = 90;
int sophia2 = 86;
int sophia3 = 87;
int sophia4 = 98;
int sophia5 = 100;

int andrew1 = 92;
int andrew2 = 89;
int andrew3 = 81;
int andrew4 = 96;
int andrew5 = 90;

int emma1 = 90;
int emma2 = 85;
int emma3 = 87;
int emma4 = 98;
int emma5 = 68;

int logan1 = 90;
int logan2 = 95;
int logan3 = 87;
int logan4 = 88;
int logan5 = 96;

int sophiaSum = 0;
int andrewSum = 0;
int emmaSum = 0;
int loganSum = 0;

decimal sophiaScore;
decimal andrewScore;
decimal emmaScore;
decimal loganScore;

sophiaSum = sophia1 + sophia2 + sophia3 + sophia4 + sophia5;
andrewSum = andrew1 + andrew2 + andrew3 + andrew4 + andrew5;
emmaSum = emma1 + emma2 + emma3 + emma4 + emma5;
loganSum = logan1 + logan2 + logan3 + logan4 + logan5;

sophiaScore = (decimal)sophiaSum / currentAssignments;
andrewScore = (decimal)andrewSum / currentAssignments;
emmaScore = (decimal)emmaSum / currentAssignments;
loganScore = (decimal)loganSum / currentAssignments;

// Console.WriteLine("Student\t\tGrade\n");
// Console.WriteLine("Sophia:\t\t" + sophiaScore + "\tA-");
// Console.WriteLine("Andrew:\t\t" + andrewScore + "\tB+");
// Console.WriteLine("Emma:\t\t" + emmaScore + "\tB");
// Console.WriteLine("Logan:\t\t" + loganScore + "\tA-");

// Console.WriteLine("Press the Enter key to continue");
// Console.ReadLine();
