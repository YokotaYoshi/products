#include <stdio.h>

#define LOWER 0
#define UPPER 300
#define STEP 20

/*fahr = 0, 20, ... , 300に対して華氏ー摂氏対応表を印字*/

main()
{
    float fahr, celsius;
    int lower, upper, step;

    lower = 0;
    upper = 300;
    step = 20;

    fahr = 0;
    while(fahr <= upper)
    {
        celsius = (5.0/9.0) * (fahr-32.0);
        //printf("%d\t%d\n", fahr, celsius);
        printf("%3.0f %6.1f\n", fahr, celsius);
        //fahrを最低3文字幅、小数点無しで印字する
        //celsiusを最低6文字幅、小数点以下1桁まで印字
        //幅を省略するなら%.2fのように
        //精度を省略するなら%5fのように
        //いずれも省略するなら%fと書く
        fahr += step;
    }

    int fahr2 = 0;
    for (fahr2 = LOWER; fahr2 <= UPPER; fahr2 += STEP)
        printf("%3d %6.1f\n", fahr2, (5.0/9.0)*(fahr2-32.0));
    
}