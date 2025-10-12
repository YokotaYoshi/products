#include <stdio.h>

#define IN  1 //単語の中
#define OUT 0 //単語の外

/*入力を出力に複写*/

main()
{
    /*
    int c;

    c = getchar();
    while (c != EOF)//EOF end of file 入力の終わりを表すビットパターン
    //ctrlDかctrlZで入力終わりに出来る。ctrlDだった
    {
        putchar(c);
        c = getchar();
    }
    
    while((c = getchar()) != EOF)
        putchar(c);
    */

    /*
    long nc;//intが2**16-1=32767までなのに対しlongは2**32-1まである
    nc = 0;
    while (getchar() != EOF)
        ++nc;
    printf("%ld\n", nc);
    */

    /*
    double nc;
    for (nc = 0; getchar() != EOF; ++nc)
        ;
    printf("%.0f\n", nc);
    */
    
    /*
    int c, nl;

    nl = 0;
    while ((c = getchar()) != EOF)
        if (c == '\n')
            ++nl;
    printf("%d\n", nl);
    */

    int c, nl, nw, nc, state;

    state = OUT;
    nl = nw = nc = 0;
    while ((c = getchar()) != EOF)
    {
        ++nc;//なんらかの入力があるたびに1追加
        if (c == '\n')
            ++nl;//改行されたら行数を1追加
        if (c == ' ' || c == '\n' || c == '\t')
            state = OUT;//空白、改行、タブが押されたら単語の外であると判定
        else if (state == OUT)//単語の外で、文字の入力があったら
        {
            state = IN;//単語の中であると判定
            ++nw;//単語数を1追加
        }
    }
    printf("行%d 単語%d 文字%d\n", nl, nw, nc);
}