#include <stdio.h>

//数字、空白文字、その他をカウント
main()
{
    int c, i, nwhite, nother;
    int ndigit[10];
    nwhite = nother = 0;
    for (i = 0; i < 10; ++i)
        ndigit[i] = 0;

    while ((c = getchar()) != EOF)
    {
        if (c >= '0' && c <= '9')//0~9の数字が出てきたら
            ++ndigit[c-'0'];//配列のc番目の数を1追加。c番目 = int c から int '0'をひく
        else if (c == ' '|| c == '\n' || c == '\t')//空白、改行、タブ
            ++nwhite;
        else//他の文字
            ++nother;
    }

    printf("digits=");
    for (i = 0; i < 10; ++i)
        printf(" %d", ndigit[i]);
    printf(", white space = %d, other = %d\n", nwhite, nother);
}