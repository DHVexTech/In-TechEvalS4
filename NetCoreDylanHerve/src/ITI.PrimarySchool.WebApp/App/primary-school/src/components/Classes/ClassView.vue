<template>
    <div>

        <label>Professeur :</label>
        <table class="table table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th>Prenom</th>
                        <th>Nom</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-if="teacher.length == 0">
                        <td colspan="4" class="text-center">Il n'y a actuellement aucun élève.</td>
                    </tr>

                    <tr v-for="i of teacher">
                        <td>{{ i.firstName }}</td>
                        <td>{{ i.lastName }}</td>
                    </tr>
                </tbody>
            </table>

        <label>Etudiants :</label>
        <table class="table table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th>Prenom</th>
                        <th>Nom</th>
                        <th>Date de naissance</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-if="student.length == 0">
                        <td colspan="4" class="text-center">Il n'y a actuellement aucun élève.</td>
                    </tr>

                    <tr v-for="i of student">
                        <td>{{ i.firstName }}</td>
                        <td>{{ i.lastName }}</td>
                        <td>{{ i.birthDate }}</td>
                    </tr>
                </tbody>
            </table>


        

    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import ClassApiService from '../../services/ClassApiService'
    import TeacherApiService from '../../services/TeacherApiService'
    import StudentApiService from '../../services/StudentApiService'

    export default {
        data () {
            return {
                item: {},
                mode: null,
                id: null,
                errors: [],
                teacher: {},
                student: {}
            }
        },

        async mounted() {
            this.mode = this.$route.params.mode;
            this.id = this.$route.params.id;

            await this.SearchStudentAndTeacher();
            console.log("coucou");
        },

        methods: {
            ...mapActions(['executeAsyncRequestOrDefault', 'executeAsyncRequest']),

            async SearchStudentAndTeacher()
            {
                this.student = await this.executeAsyncRequestOrDefault(() => StudentApiService.getStudentByClass(this.id));
                this.teacher = await this.executeAsyncRequestOrDefault(() => TeacherApiService.getTeacherByClass(this.id));
                console.log(this.teacher);
            },

            async onSubmit(e) {
                e.preventDefault();

                
                var errors = [];

               

                this.errors = errors;

                if(errors.length == 0) {
                }
            }
        }
    }
</script>

<style lang="less">

</style>