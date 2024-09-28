function nextStep(stepNumber) {
    // Hide all steps
    const steps = document.querySelectorAll('section.step');
    steps.forEach(step => step.classList.remove('active'));

    // Show the next step
    document.getElementById('step' + stepNumber).classList.add('active');
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

    if (college === 'CAF') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSA">BSA - Bachelor of Science in Accountancy</option>
            <option value="BSMA">BSMA - Bachelor of Science in Management Accounting</option>
            <option value="BSBAFM">BSBAFM - Bachelor of Science in Business Administration Major in Financial Management</option>
        `;
    } else if (college === 'CADBE') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSARCH">BSARCH - Bachelor of Science in Architecture</option>
            <option value="BSID">BSID - Bachelor of Science in Interior Design</option>
            <option value="BSEP">BSEP - Bachelor of Science in Environmental Planning</option>
        `;
    } else if (college === 'CAL') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="ABELS">ABELS - Bachelor of Arts in English Language Studies</option>
            <option value="ABF">ABF - Bachelor of Arts in Filipinology</option>
            <option value="ABLCS">ABLCS - Bachelor of Arts in Literary and Cultural Studies</option>
            <option value="ABPHILO">ABPHILO - Bachelor of Arts in Philosophy</option>
            <option value="BPEA">BPEA - Bachelor of Performing Arts major in Theater Arts</option>
        `;
    } else if (college === 'CBA') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="DBA">DBA - Doctor in Business Administration</option>
            <option value="MBA">MBA - Master in Business Administration</option>
            <option value="BSBAHRM">BSBAHRM - Bachelor of Science in Business Administration major in Human Resource Management</option>
            <option value="BSBAMM">BSBAMM - Bachelor of Science in Business Administration major in Marketing Management</option>
            <option value="BSENTREP">BSENTREP - Bachelor of Science in Entrepreneurship</option>
            <option value="BSOA">BSOA - Bachelor of Science in Office Administration</option>
        `;
    } else if (college === 'COC') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BADPR">BADPR - Bachelor in Advertising and Public Relations</option>
            <option value="BABroadcasting">BABroadcasting - Bachelor of Arts in Broadcasting</option>
            <option value="BACR">BACR - Bachelor of Arts in Communication Research</option>
            <option value="BAJ">BAJ - Bachelor of Arts in Journalism</option>
        `;
    } else if (college === 'CCIS') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSCS">BSCS - Bachelor of Science in Computer Science</option>
            <option value="BSIT">BSIT - Bachelor of Science in Information Technology</option>
        `;
    } else if (college === 'COED') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="PhDEM">PhDEM - Doctor of Philosophy in Education Management</option>
            <option value="MAEM-EL">MAEM - Master of Arts in Education Management with Specialization in Educational Leadership</option>
            <option value="MAEM-IL">MAEM - Master of Arts in Education Management with Specialization in Instructional Leadership</option>
            <option value="MBE">MBE - Master in Business Education</option>
            <option value="MLIS">MLIS - Master in Library and Information Science</option>
            <option value="MAELT">MAELT - Master of Arts in English Language Teaching</option>
            <option value="MAEd-ME">MAEd-ME - Master of Arts in Education major in Mathematics Education</option>
            <option value="MAPES">MAPES - Master of Arts in Physical Education and Sports</option>
            <option value="MAED-TCA">MAED-TCA - Master of Arts in Education major in Teaching Challenged Areas</option>
            <option value="PBDE">PBDE - Post-Baccalaureate Diploma in Education</option>
            <option value="BTLEd-HE">BTLEd-HE - Bachelor of Technology and Livelihood Education major in Home Economics</option>
            <option value="BTLEd-IA">BTLEd-IA - Bachelor of Technology and Livelihood Education major in Industrial Arts</option>
            <option value="BTLEd-ICT">BTLEd-ICT - Bachelor of Technology and Livelihood Education major in Information and Communication Technology</option>
            <option value="BLIS">BLIS - Bachelor of Library and Information Science</option>
            <option value="BSEd-E">BSEd-E - Bachelor of Science in Education major in English</option>
            <option value="BSEd-M">BSEd-M - Bachelor of Science in Education major in Mathematics</option>
            <option value="BSEd-S">BSEd-S - Bachelor of Science in Education major in Science</option>
            <option value="BSEd-F">BSEd-F - Bachelor of Science in Education major in Filipino</option>
            <option value="BSEd-SS">BSEd-SS - Bachelor of Science in Education major in Social Studies</option>
            <option value="BEEd">BEEd - Bachelor of Elementary Education</option>
            <option value="BECEd">BECEd - Bachelor of Early Childhood Education</option>
        `;
    } else if (college === 'CE') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSCE">BSCE - Bachelor of Science in Civil Engineering</option>
            <option value="BSCpE">BSCpE - Bachelor of Science in Computer Engineering</option>
            <option value="BSEE">BSEE - Bachelor of Science in Electrical Engineering</option>
            <option value="BSECE">BSECE - Bachelor of Science in Electronics Engineering</option>
            <option value="BSIE">BSIE - Bachelor of Science in Industrial Engineering</option>
            <option value="BSME">BSME - Bachelor of Science in Mechanical Engineering</option>
            <option value="BSRE">BSRE - Bachelor of Science in Railway Engineering</option>
        `;
    } else if (college === 'CHK') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BPE">BPE - Bachelor of Physical Education</option>
            <option value="BSESS">BSESS - Bachelor of Science in Exercises and Sports</option>
        `;
    } else if (college === 'CL') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="JD">JD - Juris Doctor</option>
        `;
    } else if (college === 'CPSPA') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="DPA">DPA - Doctor in Public Administration</option>
            <option value="MPA">MPA - Master in Public Administration</option>
            <option value="BPA">BPA - Bachelor of Public Administration</option>
            <option value="BAIS">BAIS - Bachelor of Arts in International Studies</option>
            <option value="BAPE">BAPE - Bachelor of Arts in Political Economy</option>
            <option value="BAPS">BAPS - Bachelor of Arts in Political Science</option>
        `;
    } else if (college === 'CSSD') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BAH">BAH - Bachelor of Arts in History</option>
            <option value="BAS">BAS - Bachelor of Arts in Sociology</option>
            <option value="BSC">BSC - Bachelor of Science in Cooperatives</option>
            <option value="BSE">BSE - Bachelor of Science in Economics</option>
            <option value="BSPSY">BSPSY - Bachelor of Science in Psychology</option>
        `;
    } else if (college === 'CS') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSFT">BSFT - Bachelor of Science in Food Technology</option>
            <option value="BSAPMATH">BSAPMATH - Bachelor of Science in Applied Mathematics</option>
            <option value="BSBIO">Bachelor of Science in Biology</option>
            <option value"BSCHEM">BSCHEM - Bachelor of Science in Chemistry</option>
            <option value="BSMATH">BSMATH - Bachelor of Science in Mathematics</option>
            <option value="BSND">BSND - Bachelor of Science in Nutrition and Dietetics</option>
            <option value="BSPHY">BSPHY - Bachelor of Science in Physics</option>
            <option value="BSSTAT">BSSTAT - Bachelor of Science in Statistics</option>
        `;
    } else if (college === 'CTHTM') {
        programSelect.innerHTML = `
            <option value="" disabled selected>Select your program</option>
            <option value="BSHM">BSHM - Bachelor of Science in Hotel and Restaurant Management</option>
            <option value="BSTM">BSTM - Bachelor of Science in Tourism Management</option>
            <option value="BSTRM">BSTRM - Bachelor of Science in Transportation Management</option>
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
