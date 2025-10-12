#include <stdio.h>
#define MAXLINE 1000 //入力桁の最大長

int max;//今まで出てきた最大長
char line[MAXLINE]; //現在の入力桁
char longest[MAXLINE]; //格納されている最長行

//int my_getline(char line[], int maxline);
//void copy(char to[], char from[]);
int my_getline(void);
void copy(void);

//最も長い入力桁を印字する
main()
{
    int len;//現在の桁の長さ
    //int max;//今まで出てきた最大長
    //char line[MAXLINE]; //現在の入力桁
    //char longest[MAXLINE]; //格納されている最長行
    extern int max;
    extern char longest[];

    max = 0;
    /*
    while ((len = my_getline(line, MAXLINE)) > 0)//文字がある間
    {
        if (len > max)//最大を更新したら
        {
            max = len;
            copy(longest, line);//lineをlongestにコピー
        }
    }
    */
    while ((len = my_getline()) > 0)//文字がある間
    {
        if (len > max)//最大を更新したら
        {
            max = len;
            copy();//lineをlongestにコピー
        }
    }
    if (max > 0)//行があった
        printf("%s", longest);

    return 0;
}

/*
//getline sに行を入れ、長さを返す
int my_getline(char s[], int lim)
{
    int c, i;

    for (i = 0; i < lim-1 && (c = getchar())!=EOF && c != '\n'; ++i)
    {
        //入力cは改行でも入力終了でもないならi番目に入力を代入
        s[i] = c;
    }
    if (c == '\n')
    {
        //改行されたら配列の最後に改行を代入
        s[i] = c;
        ++i;
    }
    s[i] = '\0';//配列の最後にnullを代入
    return i;
}
*/
//getline sに行を入れ、長さを返す
int my_getline(void)
{
    int c, i;
    extern char line[];

    for (i = 0; i < MAXLINE -1 && (c = getchar())!=EOF && c != '\n'; ++i)
    {
        //入力cは改行でも入力終了でもないならi番目に入力を代入
        line[i] = c;
    }
    if (c == '\n')
    {
        //改行されたら配列の最後に改行を代入
        line[i] = c;
        ++i;
    }
    line[i] = '\0';//配列の最後にnullを代入
    return i;
}

/*
//copy fromをtoにコピー toは十分大きいと仮定
void copy(char to[], char from[])
{
    int i;

    i = 0;
    while ((to[i] = from[i]) != '\0')
        ++i;
        //配列の最後に出てくるnullが出てくるまで数える
        //つまり配列の中身の数を数える
}
*/
//copy fromをtoにコピー toは十分大きいと仮定
void copy(void)
{
    int i;
    extern char line[], longest[];

    i = 0;
    while ((longest[i] = line[i]) != '\0')
        ++i;
        //配列の最後に出てくるnullが出てくるまで数える
        //つまり配列の中身の数を数える
}