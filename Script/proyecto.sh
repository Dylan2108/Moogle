#!/bin/bash
run() {
cd ..
dotnet watch run --project MoogleServer
cd Script
clear
}
report() {
cd ..
cd Informe
pdflatex Informe.tex
cd ..
cd Script
clear
}
slides() {
cd ..
cd Presentacion
pdflatex Presentacion.tex
cd ..
cd Script
clear
}
show_report() {
cd ..
cd Informe
if [ ! -f Informe.pdf ]; then
 report
fi
read -p "Seleccione un visualizador de pdf" visualizador
case $visualizador in
evince)
      evince Informe.pdf;;
xpdf)
     xpdf Informe.pdf;;
okular)
     okular Informe.pdf;;
*)
 xdg-open Informe.pdf;;
esac
cd ..
cd Script
clear
}
show_slides() {
cd ..
cd Presentacion
if [ ! -f Presentacion.pdf ]; then
slides
fi
read -p "Seleccione un visualizador de pdf" visualizador
case $visualizador in
evince)
      evince Presentacion.pdf;;
xpdf)
     xpdf Presentacion.pdf;;
okular)
     okular Presentacion.pdf;;
*)
 xdg-open Presentacion.pdf;;
esac
cd ..
cd Script
clear
}
clean() {
cd ..
cd Informe
rm -v *.aux
rm -v *.log
rm -v *.pdf
cd ..
cd Presentacion
rm -v *.aux
rm -v *.log
rm -v *.nav
rm -v *.out
rm -v *.pdf
rm -v *.snm
rm -v *.toc
cd ..
cd Script
clear
}
case $1 in
run)
   run;;
report)
   report;;
slides)
   slides;;
show_report)
   show_report;;
show_slides)
   show_slides;;
clean)
   clean;;
*)
   echo "Vuelva a escribir el comando(Los comandos disponibles son:run,report,slides,show_report,show_slides,clean)";;
esac

