#include <stdio.h>

int power(int m, int n);//関数の定義

/*べき乗関数のテスト*/
main()
{
    int i;
    for (i = 0; i < 10; ++i)
        printf("i=%d 2のi乗=%d -3のi乗%d\n", i, power(2, i), power(-3, i));
    return 0;//プログラムの正常終了
}

int power(int base, int n)//baseとnを受け取って
{
    int i, p;//宣言

    p = 1;
    for (i = 1; i <= n; ++i)
        p = p * base;
    return p;//mainにpを返す
}
/*
このようにもかける
int power(int base, int n)
{
    int p;

    for (p = 1; n > 0; --n)
        p = p * base;
    return p;
}
*/

//関数はどんな順序で書いても良い