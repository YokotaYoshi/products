#include <stdio.h>

/*
二分探索
binsearch: v[0] <= v[1] <= ... v[n-1]の中でxを探せ
*/

//上手くいっていない。後でやり直し

int binsearch(int x, int v[], int n)
{
    int low = 0;
    int high = n-1;
    int mid;
    while (low <= high)
    {
        mid = (low + high)/2;
        if (x < v[mid])
        {
            high = mid - 1;
        }
        else if (x > v[mid])
        {
            low = mid + 1;
        }
        else return mid;//一致した
    }
    return -1;
}

main()
{
    int c = getchar();
    int v[];
    int n = 10;
    int result;

    for (int i = 0; i < n; ++i)
    {
        v[i] = 2*i;
    }

    result = binsearch(c, v[n], n);
    printf("%d\n", result);
}