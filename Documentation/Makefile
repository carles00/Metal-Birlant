##################
# === MACROS === #
##################
VERSION := 0
PATCHLEVEL := 2
SUBLEVEL := 0
EXTRAVERSION :=

COVER := $(shell find . -type f -name "cover.ps")
SRC := $(shell find . -type f -iname "*.texi")
OUT := metal-birlant-gdd-v${VERSION}.${PATCHLEVEL}.${SUBLEVEL}${EXTRAVERSION}.pdf


########################
# === FILE TARGETS === #
########################
${OUT}: ${COVER} gdd.pdf
	@gs -dBATCH -dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=$@ $^

gdd.pdf: ${SRC}
	@texi2pdf -V --tidy gdd.texi


#########################
# === PHONY TARGETS === #
#########################
.PHONY: clean mrproper

clean:
	@rm -rf gdd.t2d/ gdd.pdf

mrproper: clean
	@rm -f metal-birlant-gdd-*.pdf
