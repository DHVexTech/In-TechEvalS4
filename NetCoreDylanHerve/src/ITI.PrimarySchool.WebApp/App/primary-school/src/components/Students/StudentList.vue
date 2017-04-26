<template>
    <div>
        <div class="page-header">
            <h1>Gestion des élèves</h1>
        </div>

        <div class="panel panel-default">
            <form @submit="onSubmit($event)">
                Rechercher un étudiant : <input type="text" v-model="studentSearch.name" class="form-control">
                <p></p><button type="submit" class="btn btn-primary">Rechercher</button>
            </form>
            <div class="panel-body text-right">
                <router-link class="btn btn-primary" :to="`students/create`"><i class="glyphicon glyphicon-plus"></i> Ajouter un élève</router-link>
            </div>
        </div>
        <span v-if='(studentSearch.name == "" || this.studentSearch.name == undefined) && search == "ok"'><alert color="red" message="Le champ est vide."></alert></span>
        <span v-if='messageDeleteStudent != null && messageDeleteStudent == "ok"'><alert color="green" message="L'étudiant a bien été supprimé."></alert></span>
        <span v-else-if='messageDeleteStudent != null && messageDeleteStudent == "failed"'><alert color="red" :message="error.responseText"></alert></span>
        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Prénom</th>
                    <th>Date de naissance</th>
                    <th>Login github</th>
                    <th>Options</th>
                    <th>Classe</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="studentList.length == 0">
                    <td colspan="6" class="text-center">Il n'y a actuellement aucun élève.</td>
                </tr>

                <tr v-for="i of studentList">
                    <td>{{ i.studentId }}</td>
                    <td>{{ i.lastName }}</td>
                    <td>{{ i.firstName }}</td>
                    <td>{{ i.birthDate }}</td>
                    <td>{{ i.gitHubLogin }}</td>
                    <td>
                        <router-link :to="`students/edit/${i.studentId}`"><i class="glyphicon glyphicon-pencil"></i></router-link>
                        <a href="#" @click="deleteStudent(i.studentId)"><i class="glyphicon glyphicon-remove"></i></a>
                        <a v-if="i.gitHubLogin != ''" href="#" @click="onFollow(i.studentId)">Follow</a>
                    </td>
                    <td>{{i.classeName}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import StudentApiService from '../../services/StudentApiService'
    import GitHubApiService from '../../services/GitHubApiService' 
    import Alert from "./../MyAlert.vue";

    export default {
        components: {
            Alert
        },

        data() {
            return {
                mode: null,
                studentList: [],
                studentSearch: {},
                errors: [],
                errorsFollow: [],
                studentIdFollow: null,
                studentFollowed: {},
                studentByPage: 5,
                studentListByPage: {},
                numberOfPage: {},
                search: null,
                messageDeleteStudent: null
            }
        },

        async mounted() {
            this.mode = this.$route.params.mode;
            await this.refreshList();
        },

        methods: {
            ...mapActions(['executeAsyncRequestOrDefault', 'executeAsyncRequest']),
            


            async refreshList() {
                this.studentList = await this.executeAsyncRequestOrDefault(() => StudentApiService.getStudentListAsync());

                this.studentFollowed = await this.executeAsyncRequest(() => GitHubApiService.getFollowingList());

                for (var i of this.studentList)
                {
                    for (var o of this.studentFollowed)
                    {
                        if (i.studentId == o.studentId)
                        {
                            i.follow = 1;
                            break;
                        } else
                            i.follow = 0;
                    }
                }
            },

            async deleteStudent(studentId) {
                try {
                    await this.executeAsyncRequest(() => StudentApiService.deleteStudentAsync(studentId));
                    await this.refreshList();
                    this.messageDeleteStudent = "ok";
                }
                catch(error) {
                    this.messageDeleteStudent = "failed";
                }
            },

            async onFollow(studentId)
            {
                try{
                    await this.executeAsyncRequest(() => GitHubApiService.followThisStudent(studentId))   
                }
                catch(error)
                {

                }
            },

            async onSubmit(e)
            {
                e.preventDefault();

                var errors = [];

                if(!this.studentSearch.name) errors.push("Name");

                this.errors = errors;
                this.search = "ok";
                console.log(this.studentSearch.name);
                if(errors.length == 0 && this.studentSearch.name != "" && this.studentSearch.name != undefined)
                {
                    this.studentList = await this.executeAsyncRequest(() => StudentApiService.getStudentFilter(this.studentSearch.name));
                }

            }
        }
    }
</script>

<style lang="less">

</style>