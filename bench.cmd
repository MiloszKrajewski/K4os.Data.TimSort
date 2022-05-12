@echo off
pushd %~dp0
mkdir .output 2> nul
cd .output
dotnet run --project ./../src/Benchmarks/ --configuration Release -- %*
popd