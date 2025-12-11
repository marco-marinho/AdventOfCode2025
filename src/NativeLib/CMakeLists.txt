cmake_minimum_required(VERSION 3.7)

project("fort_lib" LANGUAGES "Fortran" C)

add_library(fort_lib SHARED fort_lib.f90)