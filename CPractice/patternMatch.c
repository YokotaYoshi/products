#include <stdio.h>
#define MAXLINE 1000//入力行の最大長

int getline(char line[], int max);
int strindex(char source[], char searchfor[]);

char pattern[] = "ould"; //探すべきパターン

//パターンにマッチする全ての行をさがす
main()
{
    char line[MAXLINE];
    int found = 0;

    while (getline(line,MAXLINE) > 0)
    {
        if (strindex(line, pattern) >= 0)
        {
            printf("%s", line);
            found++;
        }
    }
    return found;
}

//getline: sに行を入れ、長さを返す
int getline(char s[], int lim)
{
    int c, i;

    i = 0;
    while (--lim > 0 && (c=getchar()) != EOF && c != '\n')
        s[i++] = c;
    if (c == '\n')
        s[i++] = c;
    s[i] = '\0';
    return i;
}