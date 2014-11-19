
; combined forms
(define cadr (lambda (x) (car (cdr x))))
(define caar (lambda (x) (car (car x))))
(define cddr (lambda (x) (cdr (cdr x))))

;(define-syntax or
;  (syntax-rules ()
;    ((or) #f)
;    ((or test) test)
;    ((or test1 test2 ...)
;     (let ((x test1))
;       (if x x (or test2 ...))))))
       
;(define-syntax and
;  (syntax-rules ()
;    ((and) #t)
;    ((and test) test)
;    ((and test1 test2 ...)
;     (if test1 (and test2 ...) #f))))
