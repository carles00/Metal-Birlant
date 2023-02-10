SRC := $(shell find . -type f -iname "*.texi")

gdd.pdf: ${SRC}
	@texi2pdf --tidy gdd.texi

clean:
	@rm -rf gdd.t2d/

mrproper: clean
	@rm -f gdd.pdf
