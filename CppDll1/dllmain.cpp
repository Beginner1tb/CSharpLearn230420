﻿// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"


extern "C" __declspec(dllexport) int Add(int, int);

extern "C" __declspec(dllexport) int foo1(int*);

int Add(int a, int b)
{
    return a + b;
}

int foo1(int* p)
{
    return *p;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

