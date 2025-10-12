#include <stdio.h>
#include <string.h>

//strlen: sの長さを返す

/*
int strlen(char s[])
{
    int i;
    i = 0;
    while (s[i] != '\0') i++;
    return i;
}
*/


//enum 列挙型。名前と定数値を結びつけるのに便利

enum escapes { BELL = '\a', BACKSPACE = '\b', TAB = '\t',
    NEWLINE = '\n', VTAB = '\v', RETURN = '\r'};

enum months { JAN = 1, FEB, MAR, APR, MAY, JUN, 
    JUL, AUG, SEP, OCT, NOV, DEC};
//FEB=2,MAR=3,...

main()
{
    int n, x, y;
    n = 5;
    x = n++;
    printf("n=%d x=%d\n", n, x);
    y = ++n;
    printf("n=%d y=%d\n", n, y);
}