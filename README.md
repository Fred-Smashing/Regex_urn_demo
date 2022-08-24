## Regex Urn Demo

This is a demo project inteded to demonstrate how regex can be used to validate urn's

---

### Commands availabe in the console app

- `validate arg1 arg2`
  - arg1: a urn to validate (eg: urn:isbn:100)
  - arg2: the validator to use
    - `-r` uses the regex validator
    - `-c` uses the custom validator
    
- `parse-file arg1 arg2`
  - arg1: path to the file to parse
      - `default` will use the default file included in the project
  - arg2: the validator to use
    - `-r` uses the regex validator
    - `-c` uses the custom validator
    
- `benchmark arg1`
  - arg1: the number of times to run the benchmark
    - this can be any integer value
