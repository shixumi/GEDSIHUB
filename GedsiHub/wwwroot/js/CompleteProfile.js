function validateStep(stepNumber) {
    let isValid = true;

    if (stepNumber === 1) {
        // Validate Step 1
        const firstName = document.querySelector('input[name="Input.FirstName"]');
        const middleName = document.querySelector('input[name="Input.MiddleName"]');
        const lastName = document.querySelector('input[name="Input.LastName"]');
        const dateOfBirth = document.querySelector('input[name="Input.DateOfBirth"]');
        const phoneNumber = document.querySelector('input[name="Input.PhoneNumber"]');

        if (!firstName.value.trim()) {
            isValid = false;
            firstName.classList.add('is-invalid');
        } else {
            firstName.classList.remove('is-invalid');
        }

        if (!lastName.value.trim()) {
            isValid = false;
            lastName.classList.add('is-invalid');
        } else {
            lastName.classList.remove('is-invalid');
        }

        if (!dateOfBirth.value.trim()) {
            isValid = false;
            dateOfBirth.classList.add('is-invalid');
        } else {
            dateOfBirth.classList.remove('is-invalid');
        }

        const phonePattern = /^[0-9]{11}$/;
        if (!phonePattern.test(phoneNumber.value.trim())) {
            isValid = false;
            phoneNumber.classList.add('is-invalid');
        } else {
            phoneNumber.classList.remove('is-invalid');
        }
    } else if (stepNumber === 2) {
        // Validate Step 2
        const sex = document.querySelector('select[name="Input.Sex"]');
        const genderIdentity = document.querySelector('select[name="Input.GenderIdentity"]');
        const pronouns = document.querySelector('select[name="Input.Pronouns"]');

        if (!sex.value) {
            isValid = false;
            sex.classList.add('is-invalid');
        } else {
            sex.classList.remove('is-invalid');
        }

        if (!genderIdentity.value) {
            isValid = false;
            genderIdentity.classList.add('is-invalid');
        } else {
            genderIdentity.classList.remove('is-invalid');
        }

        if (!pronouns.value) {
            isValid = false;
            pronouns.classList.add('is-invalid');
        } else {
            pronouns.classList.remove('is-invalid');
        }
    } else if (stepNumber === 3) {
        // Validate Step 3
        const isMemberOfIndigenousCommunity = document.querySelector('select[name="Input.IsMemberOfIndigenousCommunity"]');
        const isDisabled = document.querySelector('select[name="Input.IsDisabled"]');

        if (!isMemberOfIndigenousCommunity.value) {
            isValid = false;
            isMemberOfIndigenousCommunity.classList.add('is-invalid');
        } else {
            isMemberOfIndigenousCommunity.classList.remove('is-invalid');
        }

        if (!isDisabled.value) {
            isValid = false;
            isDisabled.classList.add('is-invalid');
        } else {
            isDisabled.classList.remove('is-invalid');
        }
    } else if (stepNumber === 4) {
        // Validate Step 4
        if (document.querySelector('input[name="Input.StudentInfo.CollegeDeptId"]')) {
            // Student-specific validations
            const collegeDeptId = document.querySelector('select[name="Input.StudentInfo.CollegeDeptId"]');
            const courseId = document.querySelector('select[name="Input.StudentInfo.CourseId"]');
            const year = document.querySelector('input[name="Input.StudentInfo.Year"]');
            const section = document.querySelector('input[name="Input.StudentInfo.Section"]');

            if (!collegeDeptId.value) {
                isValid = false;
                collegeDeptId.classList.add('is-invalid');
            } else {
                collegeDeptId.classList.remove('is-invalid');
            }

            if (!courseId.value) {
                isValid = false;
                courseId.classList.add('is-invalid');
            } else {
                courseId.classList.remove('is-invalid');
            }

            if (!year.value || year.value < 1) {
                isValid = false;
                year.classList.add('is-invalid');
            } else {
                year.classList.remove('is-invalid');
            }

            if (!section.value.trim()) {
                isValid = false;
                section.classList.add('is-invalid');
            } else {
                section.classList.remove('is-invalid');
            }
        } else if (document.querySelector('input[name="Input.EmployeeInfo.EmployeeType"]')) {
            // Employee-specific validations
            const employeeType = document.querySelector('select[name="Input.EmployeeInfo.EmployeeType"]');
            const employmentStatus = document.querySelector('select[name="Input.EmployeeInfo.EmploymentStatus"]');
            const sector = document.querySelector('select[name="Input.EmployeeInfo.Sector"]');
            const branchOffice = document.querySelector('select[name="Input.EmployeeInfo.BranchOfficeSectionUnit"]');
            const position = document.querySelector('input[name="Input.EmployeeInfo.Position"]');

            if (!employeeType.value) {
                isValid = false;
                employeeType.classList.add('is-invalid');
            } else {
                employeeType.classList.remove('is-invalid');
            }

            if (!employmentStatus.value) {
                isValid = false;
                employmentStatus.classList.add('is-invalid');
            } else {
                employmentStatus.classList.remove('is-invalid');
            }

            if (!sector.value) {
                isValid = false;
                sector.classList.add('is-invalid');
            } else {
                sector.classList.remove('is-invalid');
            }

            if (!branchOffice.value) {
                isValid = false;
                branchOffice.classList.add('is-invalid');
            } else {
                branchOffice.classList.remove('is-invalid');
            }

            if (!position.value.trim()) {
                isValid = false;
                position.classList.add('is-invalid');
            } else {
                position.classList.remove('is-invalid');
            }
        }
    }

    return isValid;
}

function nextStep(stepNumber) {
    if (validateStep(stepNumber - 1)) {
        // Hide all steps
        const steps = document.querySelectorAll('section.step');
        steps.forEach(step => step.classList.remove('active'));

        // Show the next step
        document.getElementById('step' + stepNumber).classList.add('active');
    }
}

function prevStep(stepNumber) {
    // Hide all steps
    const steps = document.querySelectorAll('section.step');
    steps.forEach(step => step.classList.remove('active'));

    // Show the previous step
    document.getElementById('step' + stepNumber).classList.add('active');
}


function submitForm() {
    // Validate form data and submit the form
    alert('Form submitted successfully!');
}

function showConditionalFields() {
    const role = document.getElementById('role').value;
    const employeeFields = document.getElementById('employee-fields');
    const studentFields = document.getElementById('student-fields');
    
    // Hide both sections initially
    employeeFields.style.display = 'none';
    studentFields.style.display = 'none';
    
    // Show relevant fields based on the selected role
    if (role === 'Student') {
        studentFields.style.display = 'block';
    } else if (role === 'Employee') {
        employeeFields.style.display = 'block';
    }
}

function showProgramOptions() {
    const college = document.getElementById('college').value;
    const programContainer = document.getElementById('program-container');
    const programSelect = document.getElementById('program');

    // Clear existing program options
    programSelect.innerHTML = '';

    if (college === '1') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="1">Bachelor of Science in Accountancy (BSA)</option>
        <option value="2">Bachelor of Science in Management Accounting (BSMA)</option>
        <option value="3">Bachelor of Science in Business Administration Major in Financial Management (BSBAFM)</option>
    `;
    } else if (college === '2') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="4">Bachelor of Science in Architecture (BSARCH)</option>
        <option value="5">Bachelor of Science in Interior Design (BSID)</option>
        <option value="6">Bachelor of Science in Environmental Planning (BSEP)</option>
    `;
    } else if (college === '3') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="7">Bachelor of Arts in English Language Studies (ABELS)</option>
        <option value="8">Bachelor of Arts in Filipinology (ABF)</option>
        <option value="9">Bachelor of Arts in Literary and Cultural Studies (ABLCS)</option>
        <option value="10">Bachelor of Arts in Philosophy (ABPHILO)</option>
        <option value="11">Bachelor of Performing Arts major in Theater Arts (BPEA)</option>
    `;
    } else if (college === '4') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="12">Doctor in Business Administration (DBA)</option>
        <option value="13">Master in Business Administration (MBA)</option>
        <option value="14">Bachelor of Science in Business Administration major in Human Resource Management (BSBAHRM)</option>
        <option value="15">Bachelor of Science in Business Administration major in Marketing Management (BSBAMM)</option>
        <option value="16">Bachelor of Science in Entrepreneurship (BSENTREP)</option>
        <option value="17">Bachelor of Science in Office Administration (BSOA)</option>
    `;
    } else if (college === '5') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="18">Bachelor in Advertising and Public Relations (BADPR)</option>
        <option value="19">Bachelor of Arts in Broadcasting (BABroadcasting)</option>
        <option value="20">Bachelor of Arts in Communication Research (BACR)</option>
        <option value="21">Bachelor of Arts in Journalism (BAJ)</option>
    `;
    } else if (college === '6') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="22">Bachelor of Science in Computer Science (BSCS)</option>
        <option value="23">Bachelor of Science in Information Technology (BSIT)</option>
    `;
    } else if (college === '7') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="24">Doctor of Philosophy in Education Management (PhDEM)</option>
        <option value="25">Master of Arts in Education Management with Specialization in Educational Leadership (MAEM-EL)</option>
        <option value="26">Master of Arts in Education Management with Specialization in Instructional Leadership (MAEM-IL)</option>
        <option value="27">Master in Business Education (MBE)</option>
        <option value="28">Master in Library and Information Science (MLIS)</option>
        <option value="29">Master of Arts in English Language Teaching (MAELT)</option>
        <option value="30">Master of Arts in Education major in Mathematics Education (MAEd-ME)</option>
        <option value="31">Master of Arts in Physical Education and Sports (MAPES)</option>
        <option value="32">Master of Arts in Education major in Teaching Challenged Areas (MAED-TCA)</option>
        <option value="33">Post-Baccalaureate Diploma in Education (PBDE)</option>
        <option value="34">Bachelor of Technology and Livelihood Education major in Home Economics (BTLEd-HE)</option>
        <option value="35">Bachelor of Technology and Livelihood Education major in Industrial Arts (BTLEd-IA)</option>
        <option value="36">Bachelor of Technology and Livelihood Education major in Information and Communication Technology (BTLEd-ICT)</option>
        <option value="37">Bachelor of Library and Information Science (BLIS)</option>
        <option value="38">Bachelor of Elementary Education (BEEd)</option>
        <option value="39">Bachelor of Early Childhood Education (BECEd)</option>
    `;
    } else if (college === '8') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="40">Bachelor of Science in Civil Engineering (BSCE)</option>
        <option value="41">Bachelor of Science in Computer Engineering (BSCpE)</option>
        <option value="42">Bachelor of Science in Electrical Engineering (BSEE)</option>
        <option value="43">Bachelor of Science in Electronics Engineering (BSECE)</option>
        <option value="44">Bachelor of Science in Industrial Engineering (BSIE)</option>
        <option value="45">Bachelor of Science in Mechanical Engineering (BSME)</option>
        <option value="46">Bachelor of Science in Railway Engineering (BSRE)</option>
    `;
    } else if (college === '9') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="47">Bachelor of Physical Education (BPE)</option>
        <option value="48">Bachelor of Science in Exercises and Sports (BSESS)</option>
    `;
    } else if (college === '10') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="49">Juris Doctor (JD)</option>
    `;
    } else if (college === '11') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="50">Doctor in Public Administration (DPA)</option>
        <option value="51">Master in Public Administration (MPA)</option>
        <option value="52">Bachelor of Public Administration (BPA)</option>
        <option value="53">Bachelor of Arts in International Studies (BAIS)</option>
        <option value="54">Bachelor of Arts in Political Economy (BAPE)</option>
        <option value="55">Bachelor of Arts in Political Science (BAPS)</option>
    `;
    } else if (college === '12') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="56">Bachelor of Arts in History (BAH)</option>
        <option value="57">Bachelor of Arts in Sociology (BAS)</option>
        <option value="58">Bachelor of Science in Cooperatives (BSC)</option>
        <option value="59">Bachelor of Science in Economics (BSE)</option>
        <option value="60">Bachelor of Science in Psychology (BSPSY)</option>
    `;
    } else if (college === '13') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="61">Bachelor of Science in Food Technology (BSFT)</option>
        <option value="62">Bachelor of Science in Applied Mathematics (BSAPMATH)</option>
        <option value="63">Bachelor of Science in Biology (BSBIO)</option>
        <option value="64">Bachelor of Science in Chemistry (BSCHEM)</option>
        <option value="65">Bachelor of Science in Mathematics (BSMATH)</option>
        <option value="66">Bachelor of Science in Nutrition and Dietetics (BSND)</option>
        <option value="67">Bachelor of Science in Physics (BSPHY)</option>
        <option value="68">Bachelor of Science in Statistics (BSSTAT)</option>
    `;
    } else if (college === '14') {
        programSelect.innerHTML = `
        <option value="" disabled selected>Select your program</option>
        <option value="69">Bachelor of Science in Hospitality Management (BSHM)</option>
        <option value="70">Bachelor of Science in Tourism Management (BSTM)</option>
        <option value="71">Bachelor of Science in Transportation Management (BSTRM)</option>
    `;
    }

    // Show the program field after populating options
    programContainer.style.display = 'block';
}

function showBranchOptions() {
    const sector = document.getElementById('sector').value;
    const branchOfficeContainer = document.getElementById('branch-office-container');
    const branchOfficeSelect = document.getElementById('branch-office');

    // Clear existing branch options
    branchOfficeSelect.innerHTML = '';

    // Add branch options based on selected sector
    if (sector === 'OEVP') {
        branchOfficeSelect.innerHTML = `
            <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="OVEP">Office of the Executive Vice President</option>
            <option value="ICTO">Information and Communications Technology Office</option>
            <option value="IDSA">Institute for Data and Statistical Analysis</option>
            <option value="IAO">Internal Audit Office</option>
            <option value="IMO">Inspection Management Office</option>
            <option value="OIA">Office of the International Affairs</option>
            <option value="UPP">University Printing Press</option>
        `;
    } else if (sector === 'OVPAA') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="OVPAA">Office of the Vice President for Academic Affairs</option>
            <option value="COE">College of Engineering</option>
            <option value="FEO">Faculty Evaluations Office</option>
            <option value="NSTP">National Service Training Program</option>
            <option value="CAFA">College of Architecture and Fine Arts</option>
            <option value="ITech">Institute of Technology</option>
            <option value="CS">College of Science</option>
            <option value="GS">Graduate School</option>
            <option value="CTHTM">College of Tourism, Hospitality and Transportation Management</option>
            <option value="CBA">College of Business Administration</option>
            <option value="CAF">College of Accountancy and Finance</option>
            <option value="CSSD">College of Social Sciences and Development</option>
            <option value="NALLRC">Ninoy Aquino Library and Learning Resources Center</option>
            <option value="SHS">Senior High School</option>
            <option value="OU">Open University</option>
            <option value="CCIS">College of Computer and Information Sciences</option>
            <option value="COC">College of Communication</option>
            <option value="CPSPA">College of Political Science and Public Administration</option>
            <option value="CAL">College of Arts and Letters</option>
        `;
    } else if (sector === 'OVPF') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="AD">Accounting Department</option>
            <option value="BSO">Budget Services Office</option>
            <option value="FMO">Fund Management Office</option>
            <option value="RGO">Resource Generation Office</option>
            <option value="IPO">Institutional Planning Office</option>
        `;
    } else if (sector === 'OVPSAS') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="OVPSAS">Office of the Vice President for Student Affairs and Services</option>
            <option value="ARCDO">Alumni Relations and Career Development Office</option>
            <option value="OCPS">Office of Counseling and Psychological Services</option>
            <option value="OSFA">Office of Scholarship and Financial Assistance</option>
            <option value="OSS">Office of the Student Services</option>
            <option value="OUR">Office of the University Registrar</option>
            <option value="SDPO">Sports Development Office</option>
            <option value="UCCA">University Center for Culture and the Arts</option>
        `;
    } else if (sector === 'OVPA') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="CRO">Central Records Office</option>
            <option value="FMO">Facility Management Office</option>
            <option value="GSO">General Services Office</option>
            <option value="HRMD">Human Resource Management Department</option>
            <option value="MHDPC">M.H Del Pilar Campus</option>
            <option value="MSD">Medical Services Department</option>
            <option value="PPDO">Physical Planning and Development Office</option>
            <option value="PMO">Procurement Management Office</option>
            <option value="PSMO">Philippine Society of Medical Oncology</option>
            <option value="USSO">University Safety and Security Office</option>
        `;
    } else if (sector === 'OVPC') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="OVPC">Office of the Vice President for Campuses</option>
            <option value="PUP-SPC">PUP San Pedro Campus</option>
            <option value="PUP-Mulanay">PUP Mulanay</option>
            <option value="PUP-QC">PUP Quezon City</option>
            <option value="PUP-Unisan">PUP Unisan</option>
            <option value="PUP-SJ">PUP San Juan</option>
            <option value="PUP-Alfonso">PUP Alfonso</option>
            <option value="PUP-Bataan">PUP Bataan</option>
            <option value="PUP-Sta.Maria>PUP Sta. Maria</option>
            <option value="PUP-Ragay">PUP Ragay</option>
            <option value="PUP-ST">PUP Sto. Tomas</option>
            <option value="PUP-Cabiao">PUP Cabiao</option>
        `;
    } else if (sector === 'OVPRED') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option>  
            <option value="OVPRED">Office of the Vice President for Research, Extension and Development</option>
            <option value="RMO">Research Management Office</option>
            <option value="EMO">Research Management Office</option>
            <option value="IQMSO">Institutional Quality Management System Office</option>
            <option value="IPMO">Intellectual Property Management Office</option>
            <option value="RIHSD">Research Institute for Human and Social Development</option>
            <option value="RICL">Research Institute for Culture and Language</option>
            <option value="RIST">Research Institute for Science and Technology</option>
        `
    } else if (sector === 'OPRES') {
        branchOfficeSelect.innerHTML = `
        <option value="" disabled selected>Select Branch-Office-Section-Unit</option> 
            <option value="OPRES">Office of the President</option>
            <option value="CMO">Communications Management Office</option>
            <option value="OUBC">Office of the University Board Secretary</option>
            <option value="ULCO">University Legal Counsel Office</option>
            <option value="SPPO">Special Programs and Projects Office</option>
        `
    }


    // Show the branch/office field
    branchOfficeContainer.style.display = 'block';
}

